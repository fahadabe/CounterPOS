namespace CounterPOS.Model;

public partial class WeeklySale : ObservableObject
{
    [ObservableProperty]
    private string _Day = "";
    [ObservableProperty]
    private decimal _Sale = 0;
}