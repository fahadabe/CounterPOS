namespace CounterPOS.Model;

public partial class TopSellingItemsModel : ObservableObject
{
    [ObservableProperty]
    private string _ID = "";

    [ObservableProperty]
    private bool _IsVariantProduct;

    [ObservableProperty]
    private string _Name = "";

    [ObservableProperty]
    private int _QuantitySold =0;

   
}