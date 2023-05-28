namespace CounterPOS.ViewModel;

public partial class PurchaseReportViewModel : ObservableObject
{
    private readonly IEasyPrinting _easyPrinting;
    private readonly IDialogMessages _messages;
    private readonly IPaymentTypeService _paymentTypeService;
    private readonly IPurchaseReportService _purchaseReportService;
    private readonly ISupplierService _supplierService;
    private readonly IUserManagementService _userManagementService;

    public PurchaseReportViewModel(ISupplierService supplierService, IPurchaseReportService purchaseReportService, IUserManagementService userManagementService, IPaymentTypeService paymentTypeService, IDialogMessages messages, IEasyPrinting easyPrinting)
    {
        CurrentUser = CommonProperties.CurrentUser;
        _supplierService = supplierService;
        _purchaseReportService = purchaseReportService;
        _userManagementService = userManagementService;
        _paymentTypeService = paymentTypeService;
        _messages = messages;
        _easyPrinting = easyPrinting;
    }

    #region Commands

    public AsyncCommand GetFilteredPurchaseTransactions => new AsyncCommand(ExecuteGetPurchaseTransactions, CanExecuteGetTransactions);
    public AsyncCommand<short> JustPrintInvoiceCommand => new AsyncCommand<short>(ExecuteJustPrintInvoice, CanExecuteJustPrintInvoice);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData, CanExecuteGetTransactions);

    public AsyncCommand PurchaseTransactionDeleteCommand => new AsyncCommand(ExecutePurchaseTransactionDelete, CanExecutePurchaseTransactionDelete);
    public AsyncCommand PurchaseTransactionViewCommand => new AsyncCommand(ExecutePurchaseTransactionView, CanExecutePurchaseTransactionView);
    public AsyncCommand ShowAllPurchaseCommand => new AsyncCommand(ExecuteShowAllPurchaseCommand, CanExecuteGetTransactions);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private ObservableCollection<UserModel> _ComboCashierList = new();
    [ObservableProperty]
    private ObservableCollection<PaymentTypeModel> _ComboPaymentTypeList = new();
    [ObservableProperty]
    private UserModel? _ComboSelectedCashier = new();
    [ObservableProperty]
    private PaymentTypeModel? _ComboSelectedPaymentType = new();
    [ObservableProperty]
    private SupplierModel? _ComboSelectedSupplier = new();
    [ObservableProperty]
    private ObservableCollection<SupplierModel> _ComboSupplierList = new();
    [ObservableProperty]
    private CompanyModel _CompanyDetails = new CompanyModel();
    [ObservableProperty]
    private UserModel _CurrentUser = new UserModel();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _FilterPurchaseList = new();
    [ObservableProperty]
    private DateTime _FromDate = DateTime.Now.Date;
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _MasterPurchaseList = new();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _PrintInvoiceSource = new();
    [ObservableProperty]

    private NewPurchaseModel _SelectedPurchase = new();
    [ObservableProperty]

    private ObservableCollection<NewPurchaseDetailsModel> _SelectedPurchaseDetails = new();
    [ObservableProperty]
    private DateTime _ToDate = DateTime.Now.Date;
    [ObservableProperty]
    private decimal _TotalAmountSum;
    [ObservableProperty]
    private int _TotalTransactionCount;
    [ObservableProperty]
    private string _TransactionFilterText = "";

    

    partial void OnFilterPurchaseListChanged(ObservableCollection<NewPurchaseModel> value)
    {
        MakeTransactionsSummary();
    }



    partial void OnTransactionFilterTextChanged(string value)
    {
        FilterTransactions();
    }



    #endregion Properties

    #region Command Functions

    private bool CanExecuteGetTransactions()
    {
        return CurrentUser?.ViewReports ?? false;
    }

    private bool CanExecuteJustPrintInvoice(short arg)
    {
        return CurrentUser.ViewReports && SelectedPurchase is not null;
    }

    private bool CanExecutePurchaseTransactionDelete()
    {
        return CurrentUser?.DeleteInvoice ?? false && SelectedPurchase is not null;
    }

    private bool CanExecutePurchaseTransactionView()
    {
        return CurrentUser?.ViewReports ?? false && SelectedPurchase is not null;
    }

    private async Task ExecuteGetPurchaseTransactions()
    {
        if (CurrentUser?.ViewReports ?? false)
        {
            if (ComboSelectedPaymentType is not null && ComboSelectedCashier is not null && ComboSelectedSupplier is not null)
            {
                MasterPurchaseList = await _purchaseReportService.GetPurchaseListWithAllAsync(FromDate, ToDate, ComboSelectedPaymentType.PaymentType, ComboSelectedCashier.Username, ComboSelectedSupplier.SupplierName);
                FilterTransactions();
            }
        }
    }

    private async Task ExecuteJustPrintInvoice(short obj)
    {
        if (SelectedPurchase is not null)
        {
            await PrepareDataForPrinting(SelectedPurchase);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.PurchaseInvoice, false, true, false, obj);
            PrintInvoiceSource.Clear();
            SelectedPurchaseDetails.Clear();
        }
    }

    private async Task ExecuteLoadData()
    {
        CompanyDetails = CommonProperties.CompanyDetails;
        ComboSupplierList = await _supplierService.GetAllSuppliersAsync();
        ComboCashierList = await _userManagementService.GetUserListAsync();
        ComboPaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();

        ComboSelectedPaymentType = ComboPaymentTypeList.FirstOrDefault();
        ComboSelectedSupplier = ComboSupplierList.FirstOrDefault();
        ComboSelectedCashier = ComboCashierList.FirstOrDefault();

        await ExecuteGetPurchaseTransactions();
    }

    private async Task ExecutePurchaseTransactionDelete()
    {
        if (SelectedPurchase is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete Transaction 'PUR-{SelectedPurchase.PurchaseID}'"))
            {
                await _supplierService.DeletePurchaseTransactionAsync(SelectedPurchase);
                MasterPurchaseList.Remove(SelectedPurchase);
                FilterPurchaseList.Remove(SelectedPurchase);
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecutePurchaseTransactionView()
    {
        if (SelectedPurchase is not null)
        {
            await PrepareDataForPrinting(SelectedPurchase);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.PurchaseInvoice, true, false);
            PrintInvoiceSource.Clear();
            SelectedPurchaseDetails.Clear();
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteShowAllPurchaseCommand()
    {
        MasterPurchaseList = await _purchaseReportService.GetAllPurchasesAsync(FromDate, ToDate);
        FilterTransactions();
    }

    #endregion Command Functions

    #region Private Functions

    private void FilterTransactions()
    {
        Application.Current.Dispatcher.BeginInvoke
            (new Action(() => FilterPurchaseList = MasterPurchaseList.Where(i => i.FilterAble.ToLower().Contains(TransactionFilterText.ToLower())).ToObservableCollection()
        ));
    }

    private void MakeTransactionsSummary()
    {
        if (FilterPurchaseList is not null)
        {
            TotalTransactionCount = FilterPurchaseList.Count();
            TotalAmountSum = FilterPurchaseList.Sum(x => x.GrandTotal);
        }
    }

    private async Task PrepareDataForPrinting(NewPurchaseModel obj)
    {
        if (SelectedPurchase is not null)
        {
            PrintInvoiceSource.Add
                   (new NewPurchaseModel
                   {
                       PurchaseID = obj.PurchaseID,
                       PurchaseDate = obj.PurchaseDate,
                       SupplierName = obj.SupplierName,
                       PurchaseBY = obj.PurchaseBY,
                       PaymentType = obj.PaymentType,
                       DiscountPercent = obj.DiscountPercent,
                       DiscountAmount = obj.DiscountAmount,
                       TaxPercent = obj.TaxPercent,
                       TaxAmount = obj.TaxAmount,
                       SubTotal = obj.SubTotal,
                       GrandTotal = obj.GrandTotal,
                       PurchaseNote = obj.PurchaseNote,
                       DiscountOnTotal = obj.DiscountOnTotal,
                       NewPurchaseDetails = await SetPurchaseDetailValuesForPrinting(obj),
                       CompanyDetails = SetCompanyDetails()
                   });
        }
    }

    private ObservableCollection<CompanyModel> SetCompanyDetails()
    {
        ObservableCollection<CompanyModel> companyModel = new ObservableCollection<CompanyModel>
            {
                new CompanyModel
                {
                    Name = CompanyDetails.Name,
                    StartYear = CompanyDetails.StartYear,
                    Address1 = CompanyDetails.Address1,
                    Address2 = CompanyDetails.Address2,
                    Email = CompanyDetails.Email,
                    Phone = CompanyDetails.Phone,
                    Logo = CompanyDetails.Logo,
                    PrintMessage1 = CompanyDetails.PrintMessage1,
                    PrintMessage2 = CompanyDetails.PrintMessage2
                }
            };
        return companyModel;
    }

    private async Task<ObservableCollection<NewPurchaseDetailsModel>?> SetPurchaseDetailValuesForPrinting(NewPurchaseModel obj)
    {
            ObservableCollection<NewPurchaseDetailsModel> items = new();
        if (obj is not null)
        {
            SelectedPurchaseDetails = await _supplierService.GetTransactionsDetailsBySupplierAsync(obj.PurchaseID);
            foreach (var item in SelectedPurchaseDetails)
            {
                items.Add(new NewPurchaseDetailsModel
                {
                    PurchaseID = item.PurchaseID,
                    ProductName = item.ProductName,
                    PurchasePrice = item.PurchasePrice,
                    Qty = item.Qty,
                    Total = item.Total,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = item.DiscountAmount,
                    TaxPercent = item.TaxPercent,
                    TaxAmount = item.TaxAmount
                });
            }
            
        }

        return items;
    }

    #endregion Private Functions
}