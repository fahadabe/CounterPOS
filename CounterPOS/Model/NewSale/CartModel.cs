namespace CounterPOS.Model;

public partial class CartModel : ObservableObject
{
    
    [RelayCommand]
    private void ToggleCanUserChangePrice()
    {
        CanUserEditPrice = !CanUserEditPrice;
    }

    

    

    [ObservableProperty]
    private int _ProductID;

    [ObservableProperty]
    private string _Identifier;


    [ObservableProperty]
    private string _PCTCode;


    [ObservableProperty]
    private byte[] _Picture = new byte[1];

    partial void OnPictureChanged(byte[] value)
    {
        if (value.LongLength > 0)
        {
            IsProductImageAvailable = true;
        }
    }

    [ObservableProperty]
    private bool _CanUserEditPrice = true;

    [ObservableProperty]
    private string _Name = "";

    [ObservableProperty]
    private string _Category;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private int _Qty;


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _SalePrice;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _Discount = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _DiscountPrice;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _Tax = 0;

    [ObservableProperty]
    private decimal _TaxCharged;

    [ObservableProperty]
    private bool _IsProductImageAvailable;

    private decimal _Total = 0;
    public decimal Total
    {
        get => Calculation();
        set
        {
            _Total = value;
            OnPropertyChanged(nameof(Total));
        }
    }

    [ObservableProperty]
    private bool _IsVariantProduct;

    [ObservableProperty]
    private int _SizeID;

    [ObservableProperty]
    private string _ProductSize = "";

    private decimal Calculation()
    {
        
            decimal subTotal = Qty * SalePrice;

            decimal subTotalAfterDiscount = subTotal - ((subTotal * Discount) / 100);

            decimal priceAfterDiscount = subTotal - (subTotal * Discount / 100);
            DiscountPrice = subTotal - priceAfterDiscount;

            decimal priceAfterGST = (priceAfterDiscount + (priceAfterDiscount * Tax / 100));
            TaxCharged = priceAfterGST - priceAfterDiscount;

            return priceAfterGST;
        
        
    }
}