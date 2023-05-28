namespace CounterPOS.Model;

public partial class Tabs : ObservableObject
{
    [ObservableProperty]
    private string? _Header;



    [ObservableProperty]

    private ObservableCollection<BaseProductModel> _Data = new();

    private int _ItemsCount = 0;

    public int ItemsCount
    {
        get => Data.Count; 
        set { _ItemsCount = value;  OnPropertyChanged(); }
    }       

}