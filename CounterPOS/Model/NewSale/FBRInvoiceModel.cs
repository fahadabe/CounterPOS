namespace CounterPOS.Model;

public class FBRInvoiceModel
{
    [JsonProperty(PropertyName = "InvoiceNumber")]
    public string InvoiceNumber = "";


    
    [JsonProperty(PropertyName = "POSID")]
    public Int64 POSID;


    
    [JsonProperty(PropertyName = "USIN")]
    public string USIN = "";


    
    [JsonProperty(PropertyName = "RefUSIN")]
    public string RefUSIN = "";


    
    [JsonProperty(PropertyName = "DateTime")]
    public DateTime DateTime = DateTime.Now;


    
    [JsonProperty(PropertyName = "BuyerName")]
    public string BuyerName = "";


    
    [JsonProperty(PropertyName = "BuyerNTN")]
    public string BuyerNTN = "";


    
    [JsonProperty(PropertyName = "BuyerCNIC")]
    public string BuyerCNIC = "";


    
    [JsonProperty(PropertyName = "BuyerPhoneNumber")]
    public string BuyerPhoneNumber = "";


    
    [JsonProperty(PropertyName = "TotalSaleValue")]
    public double TotalSaleValue = 0;


    
    [JsonProperty(PropertyName = "TotalQuantity")]
    public double TotalQuantity = 0;


    
    [JsonProperty(PropertyName = "TotalTaxCharged")]
    public double TotalTaxCharged = 0;


    
    [JsonProperty(PropertyName = "Discount")]
    public double Discount = 0;


    
    [JsonProperty(PropertyName = "FurtherTax")]
    public double FurtherTax = 0;


    
    [JsonProperty(PropertyName = "TotalBillAmount")]
    public double TotalBillAmount = 0;


    
    [JsonProperty(PropertyName = "PaymentMode")]
    public int PaymentMode = 0;


    
    [JsonProperty(PropertyName = "InvoiceType")]
    public int InvoiceType;

    
    [JsonProperty(PropertyName = "Items")]
    public ObservableCollection<FBRInvoiceItemsModel> Items = new();

}