namespace CounterPOS.Model;
public partial class MonthlySale : ObservableObject
{
    [ObservableProperty]
    private string month = "";

    [ObservableProperty]
    private decimal sale = 0;
}