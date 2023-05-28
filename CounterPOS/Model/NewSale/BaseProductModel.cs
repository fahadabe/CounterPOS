namespace CounterPOS.Model;

public partial class BaseProductModel : ObservableObject
{
    [ObservableProperty]
    private int _ID;

    [ObservableProperty]
    private string _Name = "";

    [ObservableProperty]
    private string _Description = "";

    [ObservableProperty]
    private string _Identifier = "";

    [ObservableProperty]
    private string _PCTCode = "";

    [ObservableProperty]
    private DateTime _AddedDate = DateTime.Now;

    [ObservableProperty]
    private string _ViewName = "";

    [ObservableProperty]
    private decimal _PurchasePrice;

    [ObservableProperty]
    private decimal _SalePrice = 0;

    [ObservableProperty]
    private string _Category = "";

    [ObservableProperty]
    private int _PurchaseQuantity = 0;

    [ObservableProperty]
    private byte[] _Picture = new byte[1];

    [ObservableProperty]
    private int _TimesSold = 0;

    [ObservableProperty]
    private bool _IsVariantProduct;

    [ObservableProperty]
    private bool _IsAvailable;

    [ObservableProperty]
    private decimal _Discount = 0;

    [ObservableProperty]
    private decimal _Tax = 0;

    [ObservableProperty]
    private int _SizeID = 0;



    private string _ProductInfo = "";

    public string ProductInfo
    {
        get
        {
            return $"{Name} - Rs {SalePrice} - {Description}";
        }
        set
        {
            _ProductInfo = value;
            OnPropertyChanged();

        }
    }

}
