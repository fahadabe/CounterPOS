namespace CounterPOS.Model;

public partial class SaleCollectionModel : ObservableObject
{
    [ObservableProperty]
    private string saleIdentifier = "";

    [ObservableProperty]

    private decimal sale = 0;

    [ObservableProperty]

    private int transactionCount = 0;

}