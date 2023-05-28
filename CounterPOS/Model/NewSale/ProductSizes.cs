namespace CounterPOS.Model;

public partial class ProductSizes : ObservableObject
{
    [ObservableProperty]
    private int _SizeID;

    [ObservableProperty]
    private int _Index = 0;

    [ObservableProperty]
    private int _ProductID;

    [ObservableProperty]
    private byte[] _Picture = new byte[1];

    [ObservableProperty]
    private string _ItemName = "";

    [ObservableProperty]
    private string _ProductSize = "";

    [ObservableProperty]
    private bool _IsVariantProduct;

    [ObservableProperty]
    private decimal _Discount = 0;

    [ObservableProperty]
    private decimal _Tax = 0;

    [ObservableProperty]
    private decimal _PurchasePrice = 0;

    [ObservableProperty]
    private decimal _SalePrice = 0;

    [ObservableProperty]
    private string _Identifier = "";

    [ObservableProperty]
    private string _PCTCode = "";

    [ObservableProperty]
    private string _Category = "";

    [ObservableProperty]
    private int _TimesSold = 0;

    [ObservableProperty]
    private int _PurchaseQuantity = 0;

    [ObservableProperty]
    private int _AddedToCartQty = 0;

    [ObservableProperty]
    private bool _ShowQuantity = false;

    partial void OnAddedToCartQtyChanged(int value)
    {
        if (value <= 0)
        {
            ShowQuantity = false;
            return;
        }
        ShowQuantity = true;
    }

}