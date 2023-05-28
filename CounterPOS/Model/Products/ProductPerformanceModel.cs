namespace CounterPOS.Model;

public partial class ProductPerformanceModel : ObservableObject
{
    [ObservableProperty]
    private string _ProductName = "";

    [ObservableProperty]
    private int _TransactionID;


    [ObservableProperty]
    private DateTime _Date = DateTime.Now;

    [ObservableProperty]
    private int _Qty = 0;


    [ObservableProperty]
    private decimal _Price = 0;


    [ObservableProperty]
    private decimal _Discount = 0;

    [ObservableProperty]

    private decimal _Tax = 0;

    [ObservableProperty]
    private decimal _Total;

    private string _Filterable = "";

    public string Filterable
    {
        get { return $"{TransactionID} - {ProductName} - {Date} - {Qty} - {Price} - {Discount} - {Tax} - {Total}"; }
        set
        {
            _Filterable = value;
            OnPropertyChanged();
            
        }
    }
}