namespace CounterPOS.Model;

public partial class BestSellingCategoryModel : ObservableObject
{
    [ObservableProperty]
    private string category = "";

    [ObservableProperty]
    private int quantitySold = 0;   
}