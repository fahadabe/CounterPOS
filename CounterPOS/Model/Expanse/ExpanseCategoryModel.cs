namespace CounterPOS.Model;

public partial class ExpanseCategoryModel : ObservableObject
{
    [ObservableProperty]
    private int _CategoryID;

    [ObservableProperty]
    private string? _Category = "";

    
}