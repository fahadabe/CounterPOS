namespace CounterPOS.Model;

public partial class SalesPerformance : ObservableObject
{
    [ObservableProperty]
    private decimal _Today = 0;

    [ObservableProperty]

    private int _TodayTransaction = 0;

    [ObservableProperty]

    private decimal _Yesterday = 0;

    [ObservableProperty]

    private int _YesterdayTransactions = 0;


    [ObservableProperty]
    private decimal _ThisWeek = 0;


    [ObservableProperty]
    private int _ThisWeekTransaction = 0;

    [ObservableProperty]

    private decimal _ThisMonth = 0;


    [ObservableProperty]
    private int _ThisMonthTransaction = 0;


    [ObservableProperty]
    private decimal _YTDCash = 0;


    [ObservableProperty]
    private int _YTSCashTransactions = 0;


    [ObservableProperty]
    private decimal _AllCash = 0;


    [ObservableProperty]
    private int _AllCashTransactions;


    [ObservableProperty]
    private decimal _TodayBank = 0;

    [ObservableProperty]
    private int _TodayBankTransactions = 0;

    [ObservableProperty]

    private decimal _ThisMonthBank = 0;

    [ObservableProperty]
    private int _ThisMonthBankTransactions = 0;


    [ObservableProperty]
    private decimal _YTDBank = 0;

    [ObservableProperty]
    private int _YTDBankTransactions = 0;

    [ObservableProperty]

    private decimal _AllBank = 0;

    [ObservableProperty]

    private int _AllBankTransactions = 0;


    [ObservableProperty]
    private int _TodayNewCustomers = 0;

   
}