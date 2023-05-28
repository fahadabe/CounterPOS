namespace CounterPOS.Model;

public class ItemReportService : IItemReportService
{
    private readonly IDialogMessages _messages;

    public ItemReportService(IDialogMessages messages)
    {
        _messages = messages;
    }

    private string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private string Q_GetItemReportAsync = "SELECT TransactionID FROM tblSaleTransactions WHERE DATE(InvoiceDate) BETWEEN @fromDate AND @toDate ORDER by TransactionID DESC";
    private string Q_GetItems = "SELECT Name, SUM(Qty) AS Qty, Price, SUM(Total) AS Total FROM tblSaleTransactionsDetails WHERE TransactionID = @id GROUP BY Name, Price";

    public async Task<ObservableCollection<ItemReportModel>> GetItemReportAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            ObservableCollection<ItemReportModel> data = new();
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var ids = await db.QueryAsync<int>(Q_GetItemReportAsync, new
                {
                    fromDate = fromDate.ToString(SQLiteDefaultDateFormat),
                    toDate = toDate.ToString(SQLiteDefaultDateFormat)
                });

                foreach (var id in ids)
                {
                    await GetItems(id, data, db);
                }
            
            return data;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ItemReportModel>();
        }
    }

    public async Task GetItems(int id, ObservableCollection<ItemReportModel> data, IDbConnection db)
    {
        try
        {
            var collection = await db.QueryAsync<ItemReportModel>(Q_GetItems, new { id });

            foreach (var item in collection)
            {
                var foundItem = data.FirstOrDefault(i => i.Name == item.Name && i.Price == item.Price);
                if (foundItem is not null)
                {
                    foundItem.Qty += item.Qty;
                }
                else
                {
                    data.Add(new ItemReportModel
                    {
                        Name = item.Name,
                        Qty = item.Qty,
                        Price = item.Price
                    });
                }
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }
}