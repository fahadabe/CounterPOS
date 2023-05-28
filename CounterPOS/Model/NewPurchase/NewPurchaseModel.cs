namespace CounterPOS.Model;

public partial class NewPurchaseModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<CompanyModel>? _CompanyDetails;

    [ObservableProperty]
    private decimal _DiscountAmount = 0;

    [ObservableProperty]
    private bool _DiscountOnTotal;

    [ObservableProperty]
    private decimal _DiscountPercent = 0;

    private string? _FilterAble;

    [ObservableProperty]
    private decimal _GrandTotal = 0;

    [ObservableProperty]
    private ObservableCollection<NewPurchaseDetailsModel>? _NewPurchaseDetails;

    [ObservableProperty]
    private string? _PaymentType;

    [ObservableProperty]
    private string? _PurchaseBY;

    [ObservableProperty]
    private DateTime _PurchaseDate = DateTime.Now;

    [ObservableProperty]
    private int _PurchaseID;

    [ObservableProperty]
    private string? _PurchaseNote = "";

    [ObservableProperty]
    private decimal _SubTotal = 0;

    [ObservableProperty]
    private int _SupplierID = 0;

    [ObservableProperty]
    private string? _SupplierName;
    [ObservableProperty]
    private decimal _TaxAmount = 0;

    [ObservableProperty]
    private decimal _TaxPercent = 0;
    [ObservableProperty]
    private TransactionType _TransactionType;
    public string? FilterAble
    {
        get { return PurchaseID + " - " + PurchaseDate + " - " + SupplierID + " - " + SupplierName + " - " + PurchaseBY + " - " + PaymentType + " - " + PurchaseNote + " - " + TransactionType.ToString() + " - " + GrandTotal.ToString(); }
        set
        {
            _FilterAble = value;
            OnPropertyChanged();
        }
    }
}