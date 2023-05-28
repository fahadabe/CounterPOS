namespace CounterPOS.Model;

public partial class MonthlyPurchaseExpanse : ObservableObject
{
    [ObservableProperty]
    private string month = "";
    [ObservableProperty]
    private decimal purchaseExpanse = 0;

}