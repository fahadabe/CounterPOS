namespace CounterPOS.Model;

public partial class TransactionDetailsModel : ObservableObject
{
    [ObservableProperty]
    [JsonProperty(PropertyName = "TransactionDetailsID")]
    private int _TransactionDetailsID;

    [ObservableProperty]
    [JsonProperty(PropertyName = "TransactionID")]
    private int _TransactionID;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Name")]
    private string _Name = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "SizeID")]
    private int _SizeID;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Qty")]
    private decimal _Qty = 0;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Price")]
    private decimal _Price = 0;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Discount")]
    private decimal _Discount = 0;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Tax")]
    private decimal _Tax = 0;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Total")]
    private decimal _Total = 0;

    [ObservableProperty]
    [JsonProperty(PropertyName = "IsVariantProduct")]
    private bool _IsVariantProduct;

    [ObservableProperty]
    [JsonProperty(PropertyName = "ProductID")]
    private int _ProductID;

}