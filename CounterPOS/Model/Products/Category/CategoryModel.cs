namespace CounterPOS.Model;

public partial class CategoryModel : ObservableObject
{
    [ObservableProperty]
    private int _CategoryID;

    [ObservableProperty]
    private string _Category = "";

}