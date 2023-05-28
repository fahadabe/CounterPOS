namespace CounterPOS.Model;

public partial class MonthlyExpanse : ObservableObject
{
    [ObservableProperty]
    private string month = "";
    [ObservableProperty]
    private decimal expanse = 0;

    
}