namespace CounterPOS.Model;

public interface ISaleReportService
{
    Task DeleteTransactionPermanentlyAsync(TransactionModel objDeleteTransaction);
    Task<ObservableCollection<TransactionModel>> GetAllTransactionsBetweenTwoDatesAsync(DateTime fromDate, DateTime toDate);
    Task<ObservableCollection<UserModel>> GetCashierListAsync();
    Task<FBRResponseModel> GetDeletedFBRResponseAsync(int TransactionID);
    Task<ObservableCollection<TransactionDetailsModel>> GetDeletedTransactionsDetailsAsync(int TransactionID);
    Task<FBRResponseModel> GetFBRResponseAsync(int TransactionID);
    Task<ObservableCollection<TransactionModel>> GetTodayTransactionsAsync();
    Task<ObservableCollection<TransactionModel>> GetTransactionsAsync();
    Task<ObservableCollection<TransactionModel>> GetTransactionsBetweenDatesWithPaymentTypeAndCashierAsync(DateTime fromDate, DateTime toDate, string? paymentType, string? cashier);
    Task<ObservableCollection<TransactionDetailsModel>> GetTransactionsDetailsAsync(int TransactionID);
    Task<ObservableCollection<TransactionModel>> GetYesterdayTransactionsAsync();
}