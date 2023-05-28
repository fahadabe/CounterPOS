namespace CounterPOS.Model;

public class FBRResponseModel
{
    [JsonProperty(PropertyName = "Code")]
    public string Code = "";

    [JsonProperty(PropertyName = "Errors")]
    public string Errors = "";

    [JsonProperty(PropertyName = "InvoiceNumber")]
    public string InvoiceNumber = "";

    [JsonProperty(PropertyName = "Response")]
    public string Response = "";

    [JsonProperty(PropertyName = "TransactionID")]
    public int TransactionID;

    
}