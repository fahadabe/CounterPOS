
namespace CounterPOS.Model;
public partial class ProductStockInfoModel : ObservableObject
{
    [ObservableProperty]
    private string _ProductName = "";



    [ObservableProperty]
    private int _TotalQuantity;


    [ObservableProperty]
    private int _SoldQuantity;


    [ObservableProperty]
    private int _InStock;

}
