namespace CounterPOS.Model;

public partial class CustomerModel : ObservableObject
{
    [ObservableProperty]
    private int id;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CustomerFull))]
    private string name = "";


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CustomerFull))]
    private string phone = "";


    [ObservableProperty]
    private string ntn = "";


    [ObservableProperty]
    private string cnic = "";


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CustomerFull))]
    private string address = "";


    [ObservableProperty]
    private string emailAddress = "";


    [ObservableProperty]
    private DateTime addedDate = DateTime.Now;


    [ObservableProperty]
    private string remarks = "";


    [ObservableProperty]
    private bool blacklist = false;

        

    public string? CustomerFull => $"{FormatName(Name)} {FormatPhone(Phone)}{Address}";
    

    private string FormatName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return "";
        }
        else
        {
            return $"{name} -";
        }
    }

    private string FormatPhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            return "";
        }
        else
        {
            return $"{phone} - ";
        }
    }
}