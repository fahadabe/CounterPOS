namespace CounterPOS.Model;

public interface IItemReportService
{
    Task<ObservableCollection<ItemReportModel>> GetItemReportAsync(DateTime fromDate, DateTime toDate);
    Task GetItems(int id, ObservableCollection<ItemReportModel> data, IDbConnection db);
}