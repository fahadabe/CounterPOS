namespace CounterPOS.Model;

public partial class TopSellingProductSizeModel : ObservableObject
{
    [ObservableProperty]
    private string _ProductID = "";

    [ObservableProperty]
    private string _ProductSize = "";


    [ObservableProperty]
    private int _TimesSold = 0;

    
}