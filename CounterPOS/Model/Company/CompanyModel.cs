

namespace CounterPOS.Model;

public partial class CompanyModel : ObservableObject
{
    [ObservableProperty]
    [JsonProperty(PropertyName = "ID")]
    private int _ID;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Name")]
    private string? _Name = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "Country")]
    private string? _Country = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "StartYear")]
    private DateTime _StartYear = DateTime.Today;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Address1")]
    private string? _Address1 = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "Address2")]
    private string? _Address2 = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "Email")]
    private string? _Email = "" ;

    [ObservableProperty]
    [JsonProperty(PropertyName = "Phone")]
    private string? _Phone = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "Logo")]
    private byte[]? _Logo;

    [ObservableProperty]
    [JsonProperty(PropertyName = "PrintMessage1")]
    private string? _PrintMessage1 = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "PrintMessage2")]
    private string? _PrintMessage2 = "";

    [ObservableProperty]
    [JsonProperty(PropertyName = "CurrencySymbol")]
    private string? _CurrencySymbol = "";
    
}