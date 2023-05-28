namespace CounterPOS.Model;

public partial class ItemReportModel : ObservableObject
{
    [ObservableProperty]
    private string? _Name;

    [ObservableProperty]
    private int _Qty = 0;

    [ObservableProperty]
    private decimal _Price;

    private decimal  _Total ;

    public decimal  Total
    {
        get { return Qty * Price; }
        set { _Total = value; OnPropertyChanged(); }
    }   

}