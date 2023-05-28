namespace CounterPOS.Model;

public class FBRInvoiceItemsModel
{

    [JsonProperty(PropertyName = "ItemCode")]
    public string ItemCode = "";

    
    [JsonProperty(PropertyName = "ItemName")]
    public string ItemName = "";

    
    [JsonProperty(PropertyName = "PCTCode")]
    public string PCTCode = "";

    
    [JsonProperty(PropertyName = "Quantity")]
    public double Quantity = 0;

    
    [JsonProperty(PropertyName = "TaxRate")]
    public double TaxRate = 0;

    
    [JsonProperty(PropertyName = "SaleValue")]
    public double SaleValue = 0;

    
    [JsonProperty(PropertyName = "Discount")]
    public double Discount = 0;

    
    [JsonProperty(PropertyName = "FurtherTax")]
    public double FurtherTax = 0;

    
    [JsonProperty(PropertyName = "TaxCharged")]
    public double TaxCharged = 0;
  
    
    
    [JsonProperty(PropertyName = "TotalAmount")]
    public double TotalAmount = 0;

    
    [JsonProperty(PropertyName = "InvoiceType")]
    public int InvoiceType;

    
    [JsonProperty(PropertyName = "RefUSIN")]
    public string RefUSIN = "";

}