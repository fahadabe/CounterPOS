namespace CounterPOS.Model;

public class NewPurchaseService : INewPurchaseService
{
    private readonly IDialogMessages _messages;
    public NewPurchaseService(IDialogMessages messages)
    {
        _messages = messages;
    }

    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_GetLatestRowFromDatabaseAsync = "SELECT max(PurchaseID) from tblPurchaseTransactions";
    private readonly string Q_InsertNewPurchaseTransactionAsync1 = "INSERT INTO tblPurchaseTransactions (PurchaseDate, SupplierID, SupplierName ,PurchaseBY ,PaymentType ,SubTotal, DiscountPercent, DiscountAmount, TaxPercent, TaxAmount, GrandTotal, PurchaseNote, DiscountOnTotal, TransactionType) VALUES (@PurchaseDate,@SupplierID,@SupplierName,@PurchaseBY,@PaymentType,@SubTotal,@DiscountPercent,@DiscountAmount,@TaxPercent,@TaxAmount,@GrandTotal,@PurchaseNote,@DiscountOnTotal,@TransactionType)";
    private readonly string Q_InsertNewPurchaseTransactionAsync2 = "INSERT INTO tblPurchaseTransactionsDetails (PurchaseID, ProductName, PurchasePrice, SalePrice, Qty, Total, DiscountPercent, DiscountAmount ,TaxPercent, TaxAmount) VALUES (@PurchaseID,@ProductName,@PurchasePrice,@SalePrice,@Qty,@Total,@DiscountPercent,@DiscountAmount,@TaxPercent,@TaxAmount)";
    int LatestRow;

    public async Task<int> GetLatestRowFromDatabaseAsync(IDbConnection db)
    {
        return LatestRow = await db.ExecuteScalarAsync<int>(Q_GetLatestRowFromDatabaseAsync);
    }

    public async Task<bool> InsertNewPurchaseTransactionAsync(NewPurchaseModel objNewPurchaseModel, ObservableCollection<NewPurchaseDetailsModel> objNewPurchaseDetailsModel)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {

                        await db.ExecuteAsync(Q_InsertNewPurchaseTransactionAsync1, new
                        {
                            PurchaseDate = objNewPurchaseModel.PurchaseDate.ToString(SQLiteDefaultDateFormat),
                            objNewPurchaseModel.SupplierID,
                            objNewPurchaseModel.SupplierName,
                            objNewPurchaseModel.PurchaseBY,
                            objNewPurchaseModel.PaymentType,
                            objNewPurchaseModel.SubTotal,
                            objNewPurchaseModel.DiscountPercent,
                            objNewPurchaseModel.DiscountAmount,
                            objNewPurchaseModel.TaxPercent,
                            objNewPurchaseModel.TaxAmount,
                            objNewPurchaseModel.GrandTotal,
                            DiscountOnTotal = objNewPurchaseModel.DiscountOnTotal ? 1 : 0,
                            objNewPurchaseModel.PurchaseNote,
                            TransactionType = objNewPurchaseModel.TransactionType.ToString()
                        });

                        await GetLatestRowFromDatabaseAsync(db);

                        foreach (var item in objNewPurchaseDetailsModel)
                        {

                            await db.ExecuteAsync(Q_InsertNewPurchaseTransactionAsync2, new
                            {
                                PurchaseID = LatestRow,
                                item.ProductName,
                                item.PurchasePrice,
                                item.SalePrice,
                                item.Qty,
                                item.Total,
                                item.DiscountPercent,
                                item.DiscountAmount,
                                item.TaxPercent,
                                item.TaxAmount,
                            });
                        }
                        transaction.Commit();
                        db.Close();
                        return transaction.ReturnSuccess();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        _messages.ShowErrorMessage(e.Message);
                        return false;
                    }
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }
}