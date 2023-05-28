namespace CounterPOS.Model;

public partial class DiscountAndSaleChartModel : ObservableObject
{
    [ObservableProperty]
    private string dataName = "";
    [ObservableProperty]
    private decimal dataValue = 0;
}