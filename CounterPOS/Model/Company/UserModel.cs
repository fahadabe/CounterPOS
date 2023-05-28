namespace CounterPOS.Model;

public partial class UserModel : ObservableObject
{
    [ObservableProperty]
    private int _UserID;

    [ObservableProperty]
    private string _Username = "";

    [ObservableProperty]
    private string _Password = "";

    [ObservableProperty]
    private bool _AddAccount;

    [ObservableProperty]
    private bool _EditAccount;

    [ObservableProperty]
    private bool _DeleteAccount;

    [ObservableProperty]
    private bool _AddInventory;

    [ObservableProperty]
    private bool _EditInventory;


    [ObservableProperty]
    private bool _DeleteInventory;


    [ObservableProperty]
    private bool _CreateInvoice;


    [ObservableProperty]
    private bool _DeleteInvoice;


    [ObservableProperty]
    private bool _AddExpanse;


    [ObservableProperty]
    private bool _EditExpanse;


    [ObservableProperty]
    private bool _DeleteExpanse;


    [ObservableProperty]
    private bool _InteractDashboard;


    [ObservableProperty]
    private bool _ViewReports;


    [ObservableProperty]
    private bool _EditSettings;

    
}