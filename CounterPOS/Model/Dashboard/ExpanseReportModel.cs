namespace CounterPOS.Model;

public partial class ExpanseReportModel : ObservableObject
{
    [ObservableProperty]
    private string categoryName = "";
    [ObservableProperty]
    private decimal amount = 0;
}