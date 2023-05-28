namespace CounterPOS.Model;

public partial class SimpleProductModel : BaseProductModel
{


    [RelayCommand]
    private void OpenStockInfoFlyout()
    {
        IsStockInfoFlyoutOpened = true;
    }

    [RelayCommand]
    private void ReloadStockInfo()
    {
       GetStockInformation(ID);
    }

    [ObservableProperty]
    private bool _IsStockInfoFlyoutOpened = false;


    [ObservableProperty]
    private int _ID;

   

    partial void OnIDChanged(int value)
    {
        GetStockInformation(value);
    }

    [ObservableProperty]
    private string _ViewName = "";

    [ObservableProperty]
    private byte[] _Picture = new byte[1];

    [ObservableProperty]
    private string _Identifier = "";

    [ObservableProperty]
    private string _PCTCode = "";

    [ObservableProperty]
    private string _Name = "";

    [ObservableProperty]
    private int _Qty = 0;

    [ObservableProperty]
    private decimal _SalePrice = 0;

    [ObservableProperty]
    private decimal _Discount = 0;

    [ObservableProperty]
    private decimal _Tax = 0;
    

    //private decimal _Total = 0;
    //public decimal Total
    //{
    //    get
    //    {
    //        return Qty * SalePrice;
    //    }
    //    set
    //    {
    //        _Total = value;
    //        OnPropertyChanged();
    //    }
    //}

    [ObservableProperty]
    private bool _IsVariantProduct;

    [ObservableProperty]
    private string _Category = "";

    [ObservableProperty]
    private int _PurchaseQuantity = 0;

    [ObservableProperty]
    private ProductStockInfoModel _ProductStockInfo = new();

    private void GetStockInformation(int id)
    {
         var soldQuantity = GetSoldQuantity(id);
         ProductStockInfo.ProductName = Name;
         ProductStockInfo.TotalQuantity = PurchaseQuantity;
         ProductStockInfo.InStock = PurchaseQuantity - soldQuantity;
         ProductStockInfo.SoldQuantity = soldQuantity;
    }

    private int GetSoldQuantity(int id = 0)
    {
        using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
        var result = db.ExecuteScalar<int>("Select TimesSold From tblProducts WHERE ID = @id", new
            {
                id
            });
            return result;
        
    }

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