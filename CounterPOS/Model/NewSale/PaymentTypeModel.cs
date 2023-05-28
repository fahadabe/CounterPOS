namespace CounterPOS.Model;

public partial class PaymentTypeModel : ObservableObject
{
    [ObservableProperty]
    private string _PaymentType = "";
}