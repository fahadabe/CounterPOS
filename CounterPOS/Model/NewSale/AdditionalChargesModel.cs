namespace CounterPOS.Model;

public partial class AdditionalChargesModel : ObservableObject
{
    [ObservableProperty]
    private int _ChargeID;

    [ObservableProperty]
    private string _ChargeType = "";


    [ObservableProperty]
    private decimal _ChargeAmount = 0;


    [ObservableProperty]
    private int _Index = 0;

    
}