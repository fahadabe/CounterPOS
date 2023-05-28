namespace CounterPOS.Model;

public partial class NewPurchaseDetailsModel : ObservableObject
{
    [ObservableProperty]
    private decimal _DiscountAmount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(TaxPercent))]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _DiscountPercent = 0;

    [ObservableProperty]
    private byte[]? _Picture;

    [ObservableProperty]
    private string? _ProductName;

    [ObservableProperty]
    private int _PurchaseID;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Qty))]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _PurchasePrice = 0;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _Qty = 0;

    [ObservableProperty]
    private decimal _SalePrice = 0;

    [ObservableProperty]
    private decimal _TaxAmount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Total))]
    private decimal _TaxPercent = 0;

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
    private decimal Calculation()

    {
        decimal subTotal = Qty * PurchasePrice;

        decimal priceAfterDiscount = subTotal - (subTotal * DiscountPercent / 100);
        DiscountAmount = subTotal - priceAfterDiscount;

        decimal priceAfterGST = (priceAfterDiscount + (priceAfterDiscount * TaxPercent / 100));
        TaxAmount = priceAfterGST - priceAfterDiscount;

        return priceAfterGST;
    }
}