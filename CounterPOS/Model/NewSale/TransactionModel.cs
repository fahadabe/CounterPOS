namespace CounterPOS;

public partial class TransactionModel : ObservableObject
{

    [ObservableProperty]
    [JsonProperty(PropertyName = "TransactionID")]
    private int _TransactionID;

    [ObservableProperty]
    [JsonProperty(PropertyName = "CustomerID")]
    private int _CustomerID;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Narration")]
    private string? _Narration;

    [ObservableProperty]
    [JsonProperty(PropertyName = "InvoiceDate")]
    private DateTime _InvoiceDate = DateTime.Today;
    
    [ObservableProperty]
    [JsonProperty(PropertyName = "InvoiceTime")]
    private string? _InvoiceTime;

    [ObservableProperty]
    [JsonProperty(PropertyName = "SubTotal")]
    private decimal _SubTotal;

    [ObservableProperty]
    [JsonProperty(PropertyName = "DiscountValue")]
    private decimal _DiscountValue;

    [ObservableProperty]
    [JsonProperty(PropertyName = "DiscountPrice")]
    private decimal _DiscountPrice;

    [ObservableProperty]
    [JsonProperty(PropertyName = "GSTValue")]
    private decimal _GSTValue;

    [ObservableProperty]
    [JsonProperty(PropertyName = "GSTPrice")]
    private decimal _GSTPrice;

    [ObservableProperty]
    [JsonProperty(PropertyName = "AdditionalCharges")]
    private decimal _AdditionalCharges;

    [ObservableProperty]
    [JsonProperty(PropertyName = "GrandTotal")]
    private decimal _GrandTotal;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Cash")]
    private decimal _Cash;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Change")]
    private decimal _Change = 0;

    [ObservableProperty]
    [JsonProperty(PropertyName = "PaymentType")]
    private string? _PaymentType;

    [ObservableProperty]
    [JsonProperty(PropertyName = "CompanyDetails")]
    private ObservableCollection<CompanyModel>? _CompanyDetails;

    [ObservableProperty]
    [JsonProperty(PropertyName = "TransactionDetail")]
    private ObservableCollection<TransactionDetailsModel>? _TransactionDetail;

    [ObservableProperty]
    [JsonProperty(PropertyName = "IsPaid")]
    private bool _IsPaid;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Cashier")]
    private string? _Cashier;

    [ObservableProperty]
    [JsonProperty(PropertyName = "IsFBRInvoice")]
    private bool _IsFBRInvoice;

    [ObservableProperty]
    [JsonProperty(PropertyName = "DiscountOnTotal")]
    private bool _DiscountOnTotal;

    [ObservableProperty]
    [JsonProperty(PropertyName = "FBRResponse")]
    private FBRResponseModel? _FBRResponse;

    [ObservableProperty]
    [JsonProperty(PropertyName = "FBRImageSource")]
    private string? _FBRImageSource;

    [ObservableProperty]
    [JsonProperty(PropertyName = "TransactionType")]
    private TransactionType _TransactionType;


    private string? _FIlterAble;

    public string? FIlterAble
    {
        get => $"{TransactionID} - {CustomerID} - {Narration} - {SubTotal.ToString()} - {AdditionalCharges.ToString()} - {GrandTotal.ToString()} - {PaymentType} - {Cashier} - {TransactionType.ToString()}";
        set
        {
            _FIlterAble = value;
            OnPropertyChanged();
        }
    }
}