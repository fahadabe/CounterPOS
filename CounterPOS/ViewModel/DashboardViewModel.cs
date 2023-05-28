namespace CounterPOS.ViewModel;

public partial class DashboardViewModel : ObservableObject
{
    private readonly IPaymentTypeService _paymentTypeService;
    private readonly IDashboardService _dashboardService;
    public DashboardViewModel(IPaymentTypeService paymentTypeService, IDashboardService dashboardService)
    {
        _paymentTypeService = paymentTypeService;
        _dashboardService = dashboardService;
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        Task.Run(() => GetPaymentTypeList());
        
    }
    
    #region Commands

    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteRefreshSale, CanExecuteRefreshSale);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private ObservableCollection<ExpanseReportModel> _AllExpanseReport = new();
    [ObservableProperty]
    private CompanyModel? _CompanyDetails = new();
    [ObservableProperty]

    private UserModel? _CurrentUser = new UserModel();
    [ObservableProperty]

    private ObservableCollection<DiscountAndSaleChartModel> _DiscountAndSaleList = new();
    [ObservableProperty]

    private DateTime _FromDate = DateTime.Today;
    [ObservableProperty]

    private ObservableCollection<MonthlyExpanse> _MonthlyExpanse = new();
    [ObservableProperty]

    private ObservableCollection<MonthlyPurchaseExpanse> _MonthlyPurchaseExpanse = new();
    [ObservableProperty]

    private ObservableCollection<MonthlySale>? _MonthlySale = new();
    [ObservableProperty]

    private string? _MonthlySalePerformanceTitle = "Showing current year data only";
    [ObservableProperty]

    private bool _OnlyCurrentYear = true;
    [ObservableProperty]

    private string? _PaymentType;
    [ObservableProperty]

    private ObservableCollection<PaymentTypeModel> _PaymentTypeList;
    [ObservableProperty]

    private ObservableCollection<SaleCollectionModel> _SaleCollection;
    [ObservableProperty]

    private SalesPerformance? _SalePerformance = new();
    [ObservableProperty]

    private PaymentTypeModel? _SelectedPaymentType = new();
    [ObservableProperty]

    private ObservableCollection<ExpanseReportModel> _ThisMonthExpanseReport = new();
    [ObservableProperty]

    private DateTime _ToDate = DateTime.Today;
    [ObservableProperty]

    private ObservableCollection<ExpanseReportModel> _TodayExpanseReport = new();
    [ObservableProperty]

    private ObservableCollection<TopSellingItemsModel> _TopSellingItems = new();
    [ObservableProperty]

    private ObservableCollection<WeeklySale> _WeeklySale = new();
    [ObservableProperty]

    private ObservableCollection<ExpanseReportModel> _YesterdayExpanseReport = new();


    partial void OnOnlyCurrentYearChanging(bool value)
    {
        if (CanExecuteRefreshSale())
        {
            if (OnlyCurrentYear)
            {
                MonthlySalePerformanceTitle = "Showing current year data only";
            }
            else
            {
                MonthlySalePerformanceTitle = "Showing current and previous years data";
            }
            GenerateMonthlySaleExpanseAndPurchaseChartData();
        }
    }

    #endregion Properties

    #region Command Functions

    private bool CanExecuteRefreshSale()
    {
        return CurrentUser.InteractDashboard;
    }

    private async Task ExecuteRefreshSale()
    {
        if (SelectedPaymentType is not null && CurrentUser.InteractDashboard)
        {
            SaleCollection = await _dashboardService.GetCashSale(SelectedPaymentType.PaymentType);
            GenerateMonthlySaleExpanseAndPurchaseChartData();
            await GenerateExpanseChartData();
            await GenerateTopSellingitems();
            await GenerateTodaySaleAndDiscountChartData();
            await GenerateWeeklySaleChartData();
        }
    }
    

    #endregion Command Functions

    #region Private Functions

    private async Task GenerateExpanseChartData()
    {
        TodayExpanseReport.Clear();
        YesterdayExpanseReport.Clear();
        ThisMonthExpanseReport.Clear();
        AllExpanseReport.Clear();
        TodayExpanseReport = await _dashboardService.GetTodayExpanseReport();
        YesterdayExpanseReport = await _dashboardService.GetYesterdayExpanseReport();
        ThisMonthExpanseReport = await _dashboardService.GetThisMonthExpanseReport();
        AllExpanseReport = await _dashboardService.GetAllExpanseReportAsync();
    }

    private void GenerateMonthlySaleExpanseAndPurchaseChartData()
    {
        MonthlySale.Clear();
        MonthlyExpanse.Clear();
        MonthlyPurchaseExpanse.Clear();
        MonthlyPurchaseExpanse =  _dashboardService.GetMonthlyPurchaseExpanse(OnlyCurrentYear, SelectedPaymentType.PaymentType);
        MonthlyExpanse =  _dashboardService.GetMonthlyExpanse(OnlyCurrentYear);
        MonthlySale =  _dashboardService.GetMonthlySale(OnlyCurrentYear, SelectedPaymentType.PaymentType);
    }

    private async Task GenerateTodaySaleAndDiscountChartData()
    {
        DiscountAndSaleList.Clear();
        DiscountAndSaleList.Add(new DiscountAndSaleChartModel { DataName = "Today's Sale", DataValue = await _dashboardService.GetTodaySale(SelectedPaymentType.PaymentType) });
        DiscountAndSaleList.Add(new DiscountAndSaleChartModel { DataName = "Today's Discount", DataValue = await _dashboardService.GetTodaysDiscountAsync(SelectedPaymentType.PaymentType) });
    }

    private async Task GenerateTopSellingitems()
    {
        TopSellingItems.Clear();
        TopSellingItems = await _dashboardService.GetTopSellingItems();
    }

    private async Task GenerateWeeklySaleChartData()
    {
        WeeklySale.Clear();
        WeeklySale = await _dashboardService.GetWeeklySale(SelectedPaymentType.PaymentType);
    }

    private async Task GetPaymentTypeList()
    {
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
        if (PaymentTypeList is not null && PaymentTypeList.Count > 0)
        {
            SelectedPaymentType = PaymentTypeList?.FirstOrDefault(i => i.PaymentType == Settings.Default.DefaultPaymentType);
        }
    }

    #endregion Private Functions
}