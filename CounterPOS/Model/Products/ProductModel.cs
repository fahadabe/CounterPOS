namespace CounterPOS.Model;

public partial class ProductModel : ObservableObject
{
    [ObservableProperty]
    private string _ViewName = "";

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
    private int _PurchaseQuantity = 0;

    [ObservableProperty]
    private DateTime _AddedDate = DateTime.Now;

    [ObservableProperty]
    private decimal _PurchasePrice = 0;

    [ObservableProperty]
    private decimal _SalePrice = 0;

    [ObservableProperty]
    private string _Category = "";

    [ObservableProperty]
    private byte[] _Picture = new byte[1];

    [ObservableProperty]
    private int _TimesSold;

    [ObservableProperty]
    private bool _IsVariantProduct;

    partial void OnIsVariantProductChanged(bool value)
    {
        CheckIfVariant(value);
    }

    [ObservableProperty]
    private bool _IsAvailable = true;


    [ObservableProperty]
    private decimal _Discount = 0;


    [ObservableProperty]
    private decimal _Tax = 0;


    
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

    [ObservableProperty]
    private int _SizeID;

    private decimal oldSalePrice;
    private decimal oldPurchaseprice;
    private int oldPurchaseQuantity;
    private string oldPCTCode;
    private string oldIdentifier;

    private void CheckIfVariant(bool value)
    {
        

        if (value)
        {
            oldSalePrice = SalePrice;
            oldPurchaseprice = PurchasePrice;
            oldPurchaseQuantity = PurchaseQuantity;
            oldPCTCode = PCTCode;
            oldIdentifier = Identifier;

            SalePrice = 0;
            PurchasePrice = 0;
            PurchaseQuantity = 0;
            PCTCode = "";
            Identifier = "";
        }
        else
        {
            SalePrice = oldSalePrice;
            PurchasePrice = oldPurchaseprice;
            PurchaseQuantity = oldPurchaseQuantity;
            PCTCode = oldPCTCode;
            Identifier = oldIdentifier;
        }
    }
}