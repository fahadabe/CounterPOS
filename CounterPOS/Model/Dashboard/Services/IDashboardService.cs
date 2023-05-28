namespace CounterPOS.Model;

public interface IDashboardService
{
    Task<ObservableCollection<ExpanseReportModel>> GetAllExpanseReportAsync();
    Task<ObservableCollection<SaleCollectionModel>> GetCashSale(string paymentType);
    ObservableCollection<MonthlyExpanse> GetMonthlyExpanse(bool OnlyCurrentYear);
    ObservableCollection<MonthlyPurchaseExpanse> GetMonthlyPurchaseExpanse(bool OnlyCurrentYear, string paymentType);
    ObservableCollection<MonthlySale> GetMonthlySale(bool OnlyCurrentYear, string paymentType);
    Task<ObservableCollection<ExpanseReportModel>> GetThisMonthExpanseReport();
    Task<ObservableCollection<ExpanseReportModel>> GetTodayExpanseReport();
    Task<decimal> GetTodaySale(string paymentType);
    Task<decimal> GetTodaysDiscountAsync(string paymentType);
    Task<int> GetTodayTransactionsCount(string paymentType);
    Task<ObservableCollection<TopSellingItemsModel>> GetTopSellingItems();
    Task<ObservableCollection<TopSellingProductSizeModel>> GetTopSellingSizes(string productID, IDbConnection db);
    Task<ObservableCollection<WeeklySale>> GetWeeklySale(string? paymentType);
    Task<ObservableCollection<ExpanseReportModel>> GetYesterdayExpanseReport();
}