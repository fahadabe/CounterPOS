namespace CounterPOS.Model;

public partial class MultiSizeProductModel : BaseProductModel
{

    [RelayCommand]
    private void OpenStockInfoFlyout()
    {
        IsStockInfoFlyoutOpened = true;
    }

    [RelayCommand]
    private void ReloadStockInfo()
    {
        if (ProductSizes is not null && ProductSizes.Count > 0)
        {
            GetStockInformation(ProductSizes);
        }
    }

    [ObservableProperty]
    private bool _IsStockInfoFlyoutOpened = false;

    [ObservableProperty]
    private int _SizeID = 0;


    [ObservableProperty]
    private int _ID = 0;


    [ObservableProperty]
    private string _ViewName = "";


    //[ObservableProperty]
    //private string _Identifier = "";

    //[ObservableProperty]
    //private string _PCTCode = "";


    [ObservableProperty]
    private byte[] _Picture = new byte[1];


    [ObservableProperty]
    private string _Name = "";

    [ObservableProperty]
    //[NotifyPropertyChangedFor(nameof(Total))]
    private decimal _Qty = 0;


    //[ObservableProperty]
    ////[NotifyPropertyChangedFor(nameof(Total))]
    //private decimal _SalePrice = 0;


    //[ObservableProperty]
    //private decimal _Discount = 0;


    //[ObservableProperty]
    //private decimal _Tax = 0;


    
    //private decimal _Total = 0;

    //public decimal Total
    //{
    //    get => Qty * SalePrice;
        
    //    set
    //    {
    //        _Total = value;
    //        OnPropertyChanged();
    //    }
    //}

    [ObservableProperty]
    private bool _IsVariantProduct = true;


    [ObservableProperty]
    private ObservableCollection<ProductSizes> _ProductSizes = new();

    partial void OnProductSizesChanged(ObservableCollection<ProductSizes> value)
    {
        GetStockInformation(value);
    }


    [ObservableProperty]
    private ObservableCollection<ProductStockInfoModel> _ProductStockInfo = new();


    [ObservableProperty]
    private string _Category = "";

    

    private void GetStockInformation(ObservableCollection<ProductSizes> productSizes)
    {
        ProductStockInfo.Clear();
        foreach (var item in productSizes)
        {
            var soldQuantity =  GetSoldQuantity(item);
            ProductStockInfo.Add(new ProductStockInfoModel
            {
                ProductName = $"{item.ItemName} {item.ProductSize}",
                TotalQuantity = item.PurchaseQuantity,
                InStock = item.PurchaseQuantity - soldQuantity,
                SoldQuantity = soldQuantity,
            });
        }
    }

    private int GetSoldQuantity(ProductSizes productSize)
    {
        var size = productSize.ProductSize;
        var id = productSize.ProductID;
        using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
        var result =  db.ExecuteScalar<int>("Select TimesSold From tblProductSize WHERE ProductID = @id AND ProductSize = @size", new
            {
                id,
                size
            });
            return result;
        
    }
}