namespace CounterPOS.Model;

public partial class ExpanseModel : ObservableObject
{
    [ObservableProperty]
    private int _ExpansesID;

    [ObservableProperty]
    private string _ExpansesDescription = "";

    [ObservableProperty]
    private decimal _Amount = 0;

    [ObservableProperty]
    private DateTime _ExpanseDate = DateTime.Now;

    [ObservableProperty]
    private string _ExpanseCategory = "";

    [ObservableProperty]
    private string _Note = "";

    public string FilterAble => $"{ExpansesDescription} - {Amount}  - {ExpanseDate} - {ExpanseCategory} -  {Note}";
   
}