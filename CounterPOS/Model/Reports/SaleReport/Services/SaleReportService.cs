namespace CounterPOS.Model;

public class SaleReportService : ISaleReportService
{
    private readonly IDialogMessages _messages;
    public SaleReportService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_GetTransactionsAsync = "SELECT* from tblSaleTransactions ORDER by TransactionID DESC";
    private readonly string Q_GetCashierListAsync = "SELECT (Username) from tblUser";
    private readonly string Q_GetAllTransactionsBetweenTwoDatesAsync = "SELECT * FROM tblSaleTransactions WHERE DATE(InvoiceDate) BETWEEN @fromDate AND @toDate ORDER by TransactionID DESC";
    private readonly string Q_GetTransactionsBetweenDatesWithPaymentTypeAndCashierAsync = "SELECT * FROM tblSaleTransactions WHERE DATE(InvoiceDate) BETWEEN @fromDate AND @toDate AND PaymentType = @paymentType AND Cashier = @cashier ORDER by TransactionID DESC";
    private readonly string Q_GetTransactionsDetailsAsync = "Select * From tblSaleTransactionsDetails Where TransactionID = @TransactionID";
    private readonly string Q_GetDeletedTransactionsDetailsAsync = "Select * From tblDeletedSaleDetails Where TransactionID = @TransactionID";
    private readonly string Q_GetTodayTransactionsAsync = "SELECT * from tblSaleTransactions WHERE InvoiceDate = DATE('now', 'localtime') ORDER by TransactionID DESC";
    private readonly string Q_GetYesterdayTransactionsAsync = "SELECT * from tblSaleTransactions WHERE InvoiceDate = DATE('now', '-1 days', 'localtime') ORDER by TransactionID DESC";
    private readonly string Q_GetFBRResponseAsync = "SELECT * FROM tblFBRResponse Where TransactionID = @TransactionID";
    private readonly string Q_GetDeletedFBRResponseAsync = "SELECT * FROM tblDeletedFBRResponse Where TransactionID = @TransactionID";
    private readonly string Q_DeleteTransactionPermanentlyAsync1 = "DELETE FROM tblDeletedSales WHERE TransactionID = @a";
    private readonly string Q_DeleteTransactionPermanentlyAsync2 = "DELETE FROM tblDeletedSaleDetails WHERE TransactionID = @b";
    private readonly string Q_DeleteTransactionPermanentlyAsync3 = "DELETE FROM tblDeletedFBRResponse WHERE TransactionID = @b";
    public async Task<ObservableCollection<TransactionModel>> GetTransactionsAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TransactionModel>(Q_GetTransactionsAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionModel>();
        }
    }

    public async Task<ObservableCollection<UserModel>> GetCashierListAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<UserModel>(Q_GetCashierListAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<UserModel>();
        }
    }

    public async Task<ObservableCollection<TransactionModel>> GetAllTransactionsBetweenTwoDatesAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TransactionModel>(Q_GetAllTransactionsBetweenTwoDatesAsync, new
                {
                    fromDate = fromDate.ToString(SQLiteDefaultDateFormat),
                    toDate = toDate.ToString(SQLiteDefaultDateFormat)
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionModel>();
        }
    }

    public async Task<ObservableCollection<TransactionModel>> GetTransactionsBetweenDatesWithPaymentTypeAndCashierAsync(DateTime fromDate, DateTime toDate, string? paymentType, string? cashier)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TransactionModel>(Q_GetTransactionsBetweenDatesWithPaymentTypeAndCashierAsync, new
                {
                    fromDate = fromDate.ToString(SQLiteDefaultDateFormat),
                    toDate = toDate.ToString(SQLiteDefaultDateFormat),
                    paymentType,
                    cashier
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionModel>();
        }
    }

    public async Task<ObservableCollection<TransactionDetailsModel>> GetTransactionsDetailsAsync(int TransactionID)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TransactionDetailsModel>(Q_GetTransactionsDetailsAsync, new
                {
                    TransactionID
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionDetailsModel>();
        }
    }

    public async Task<ObservableCollection<TransactionDetailsModel>> GetDeletedTransactionsDetailsAsync(int TransactionID)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TransactionDetailsModel>(Q_GetDeletedTransactionsDetailsAsync, new
                {
                    TransactionID
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionDetailsModel>();
        }
    }

    public async Task<ObservableCollection<TransactionModel>> GetTodayTransactionsAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TransactionModel>(Q_GetTodayTransactionsAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionModel>();
        }
    }

    public async Task<ObservableCollection<TransactionModel>> GetYesterdayTransactionsAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<TransactionModel>(Q_GetYesterdayTransactionsAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionModel>();
        }
    }

    public async Task<FBRResponseModel> GetFBRResponseAsync(int TransactionID)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QuerySingleAsync<FBRResponseModel>(Q_GetFBRResponseAsync, new
                {
                    TransactionID
                });
                return result;
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return null;
        }
    }

    public async Task<FBRResponseModel> GetDeletedFBRResponseAsync(int TransactionID)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.QuerySingleAsync<FBRResponseModel>(Q_GetDeletedFBRResponseAsync, new
                {
                    TransactionID
                });
                return result;
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return null;
        }
    }

    public async Task DeleteTransactionPermanentlyAsync(TransactionModel objDeleteTransaction)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
            using IDbTransaction transaction = db.BeginTransaction();
                    try
                    {

                        await db.ExecuteAsync(Q_DeleteTransactionPermanentlyAsync1, new
                        {
                            a = objDeleteTransaction.TransactionID
                        });


                        await db.ExecuteAsync(Q_DeleteTransactionPermanentlyAsync2, new
                        {
                            b = objDeleteTransaction.TransactionID
                        });

                        if (objDeleteTransaction.IsFBRInvoice)
                        {

                            await db.ExecuteAsync(Q_DeleteTransactionPermanentlyAsync3, new
                            {
                                b = objDeleteTransaction.TransactionID
                            });
                        }



                        transaction.Commit();
                        if (transaction.ReturnSuccess())
                        {
                            _messages.ShowSuccessNotification("Transaction Deleted");
                        }
                        else
                        {
                            _messages.ShowErrorNotification();
                        }
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        _messages.ShowErrorMessage(e.Message);
                    }
                    finally
                    {
                        db.Close();
                        transaction.Dispose();
                    }
              
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }


}