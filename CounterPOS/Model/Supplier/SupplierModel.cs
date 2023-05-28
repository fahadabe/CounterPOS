namespace CounterPOS.Model;

public partial class SupplierModel : ObservableObject
{
    [ObservableProperty]
    private int _SupplierID;


    [ObservableProperty]
    private string _SupplierName = "";


    [ObservableProperty]
    private string _Email = "";


    [ObservableProperty]
    private string _Phone = "";


    [ObservableProperty]
    private string _Location = "";


    [ObservableProperty]
    private string _Website = "";


    [ObservableProperty]
    private string _Note = "";

    [ObservableProperty]

    private DateTime _AddedDate = DateTime.Now;

    

    private string _SupplierFull = "";

    public string? SupplierFull
    {
        get { return $"{SupplierName} - {Phone} - {Email} - {Location}"; }
        set { _SupplierFull = value; OnPropertyChanged(); }
    }
}