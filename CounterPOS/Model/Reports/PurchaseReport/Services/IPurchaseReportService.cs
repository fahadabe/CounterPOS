namespace CounterPOS.Model;

public interface IPurchaseReportService
{
    Task<ObservableCollection<NewPurchaseModel>> GetAllPurchasesAsync(DateTime fromDate, DateTime toDate);
    Task<ObservableCollection<NewPurchaseModel>> GetPurchaseListWithAllAsync(DateTime fromDate, DateTime toDate, string paymentType, string cashier, string supplier);
    Task<ObservableCollection<NewPurchaseModel>> GetTodayPurchaseTransactionsAsync();
    Task<ObservableCollection<NewPurchaseModel>> GetYesterdayPurchaseTransactionsAsync();
}