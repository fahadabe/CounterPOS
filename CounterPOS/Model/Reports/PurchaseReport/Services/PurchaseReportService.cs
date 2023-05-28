namespace CounterPOS.Model;

public class PurchaseReportService : IPurchaseReportService
{
    private readonly IDialogMessages _messages;
    public PurchaseReportService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_GetTodayPurchaseTransactionsAsync = "SELECT* from tblPurchaseTransactions WHERE PurchaseDate = DATE('now', 'localtime') ORDER by PurchaseID DESC";
    private readonly string Q_GetYesterdayPurchaseTransactionsAsync = "SELECT * from tblPurchaseTransactions WHERE PurchaseDate = DATE('now', '-1 days', 'localtime') ORDER by PurchaseID DESC";
    private readonly string Q_GetAllPurchasesAsync = "SELECT * FROM tblPurchaseTransactions WHERE DATE(PurchaseDate) BETWEEN @fromDate AND @toDate ORDER by PurchaseID DESC";
    private readonly string Q_GetPurchaseListWithAllAsync = "SELECT * FROM tblPurchaseTransactions WHERE DATE(PurchaseDate) BETWEEN @fromDate AND @toDate AND PaymentType = @paymentType AND PurchaseBY = @cashier AND SupplierName = @supplier ORDER by PurchaseID DESC";
    public async Task<ObservableCollection<NewPurchaseModel>> GetTodayPurchaseTransactionsAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<NewPurchaseModel>(Q_GetTodayPurchaseTransactionsAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<NewPurchaseModel>();
        }
    }

    public async Task<ObservableCollection<NewPurchaseModel>> GetYesterdayPurchaseTransactionsAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<NewPurchaseModel>(Q_GetYesterdayPurchaseTransactionsAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<NewPurchaseModel>();
        }
    }

    public async Task<ObservableCollection<NewPurchaseModel>> GetAllPurchasesAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<NewPurchaseModel>(Q_GetAllPurchasesAsync, new
                {
                    fromDate = fromDate.ToString(SQLiteDefaultDateFormat),
                    toDate = toDate.ToString(SQLiteDefaultDateFormat)
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<NewPurchaseModel>();
        }
    }

    public async Task<ObservableCollection<NewPurchaseModel>> GetPurchaseListWithAllAsync(DateTime fromDate, DateTime toDate, string paymentType, string cashier, string supplier)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<NewPurchaseModel>(Q_GetPurchaseListWithAllAsync, new
                {
                    fromDate = fromDate.ToString(SQLiteDefaultDateFormat),
                    toDate = toDate.ToString(SQLiteDefaultDateFormat),
                    paymentType,
                    cashier,
                    supplier
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<NewPurchaseModel>();
        }
    }
}