namespace CounterPOS.ViewModel;

public partial class NewPurchaseViewModel : ObservableObject
{
    private readonly IEasyPrinting _easyPrinting;
    private readonly IDialogMessages _messages;
    private readonly INewPurchaseService _newPurchaseService;
    private readonly INewSaleService _newSaleService;
    private readonly IPaymentTypeService _paymentTypeService;
    private readonly IPurchaseReportService _purchaseReportService;
    private readonly ISupplierService _supplierService;
    private readonly IUserManagementService _userManagementService;

    public NewPurchaseViewModel(IPurchaseReportService purchaseReportService, IPaymentTypeService paymentTypeService, IUserManagementService userManagementService, INewSaleService newSaleService, INewPurchaseService newPurchaseService, ISupplierService supplierService, IDialogMessages messages, IEasyPrinting easyPrinting)
    {
        _purchaseReportService = purchaseReportService;
        _paymentTypeService = paymentTypeService;
        _userManagementService = userManagementService;
        _newSaleService = newSaleService;
        _newPurchaseService = newPurchaseService;
        _supplierService = supplierService;
        _messages = messages;
        _easyPrinting = easyPrinting;
    }
   
    #region Global Variables

    private readonly DispatcherTimer dt = new DispatcherTimer();

    #endregion Global Variables

    #region Commands

    public DelegateCommand AddToPurchaseListCommand => new DelegateCommand(ExecuteAddToPurchaselist);
    public DelegateCommand ClearCartListCommand => new DelegateCommand(ExecuteClearCartList, CanExecuteClearCartList);
    public DelegateCommand ClearSelectedSupplierCommand => new DelegateCommand(ExecuteClearSelectedSupplier);
    public DelegateCommand<NewPurchaseDetailsModel> DecreaseDiscountCommand => new DelegateCommand<NewPurchaseDetailsModel>(ExecuteDecreaseDiscount);
    public DelegateCommand<NewPurchaseDetailsModel> DecreaseQuantityCommand => new DelegateCommand<NewPurchaseDetailsModel>(ExecuteDecreaseQuantity);
    public DelegateCommand<NewPurchaseDetailsModel> DecreaseTaxCommand => new DelegateCommand<NewPurchaseDetailsModel>(ExecuteDecreaseTax);

    public AsyncCommand<string> GetTransactionsCommand => new AsyncCommand<string>(ExecuteRefreshTransactions);
    public DelegateCommand HideViewCommand => new DelegateCommand(ExecuteHideView);
    public DelegateCommand<NewPurchaseDetailsModel> IncreaseDiscountCommand => new DelegateCommand<NewPurchaseDetailsModel>(ExecuteIncreaseDiscount);
    public DelegateCommand<NewPurchaseDetailsModel> IncreaseQuantityCommand => new DelegateCommand<NewPurchaseDetailsModel>(ExecuteIncreaseQuantity);
    public DelegateCommand<NewPurchaseDetailsModel> IncreaseTaxCommand => new DelegateCommand<NewPurchaseDetailsModel>(ExecuteIncreaseTax);
    public AsyncCommand<short> JustPrintInvoiceCommand => new AsyncCommand<short>(ExecuteJustPrintInvoice, CanExecuteJustPrintInvoice);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public AsyncCommand PurchaseTransactionDeleteCommand => new AsyncCommand(ExecutePurchaseTransactionDelete, CanExecutePurchaseTransactionDelete);
    public AsyncCommand PurchaseTransactionViewCommand => new AsyncCommand(ExecutePurchaseTransactionView, CanExecutePurchaseTransactionView);
    public AsyncCommand RefreshPaymentTypeCommand => new AsyncCommand(ExecuteRefreshPaymentType);
    public DelegateCommand<NewPurchaseDetailsModel> RemoveItemFromPurchaseListCommand => new DelegateCommand<NewPurchaseDetailsModel>(ExecuteRemoveItemFromPurchaseList);
    public DelegateCommand ResetAllCommand => new DelegateCommand(ExecuteResetAll);
    public AsyncCommand SaveNewPurchaseCommand => new AsyncCommand(ExecuteInsertNewPurchase, CanExecuteNewPurchase);
    public AsyncCommand SaveNewSupplierCommand => new AsyncCommand(ExecuteSaveNewSupplier, CanExecuteSaveNewSupplier);
    public DelegateCommand ShowNewSupplierViewCommand => new DelegateCommand(ExecuteShowNewSupplierView);
    public AsyncCommand ShowTransactionsCommand => new AsyncCommand(ExecuteShowTransactions);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private ObservableCollection<UserModel> _ComboCashierList = new();
    [ObservableProperty]
    private ObservableCollection<SupplierModel> _ComboSupplierList = new();
    [ObservableProperty]
    private CompanyModel _CompanyDetails = new();
    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private bool _DiscountAndTaxOnTotal = true;
    [ObservableProperty]
    private bool _DiscountAndTaxPerItem;
    [ObservableProperty]
    private ObservableCollection<BaseProductModel> _FilteredProductList = new();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _FilteredTransactionsList = new();
    [ObservableProperty]
    private bool _IsNewSupplierViewVisible = false;
    [ObservableProperty]
    private SupplierModel _ListSelectedSupplier = new();
    [ObservableProperty]

    private ObservableCollection<BaseProductModel> _MasterProductList = new();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _MasterTransactionsList = new();
    [ObservableProperty]

    private NewPurchaseModel _NewPurchase = new();
    [ObservableProperty]
    private SupplierModel _NewSupplier = new();
    [ObservableProperty]
    private ObservableCollection<PaymentTypeModel> _PaymentTypeList = new();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _PrintInvoiceSource = new();
    [ObservableProperty]
    private string _ProductSearchTerm = "";
    [ObservableProperty]
    private ObservableCollection<NewPurchaseDetailsModel> _PurchaseCartList = new();
    [ObservableProperty]
    private int _PurchaselistCount;
    [ObservableProperty]
    private decimal _PurchaselistQuantity;
    [ObservableProperty]
    private PaymentTypeModel _SelectedPaymentType = new();
    [ObservableProperty]
    private BaseProductModel _SelectedProduct = new();
    [ObservableProperty]
    private NewPurchaseModel _SelectedPurchase = new();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseDetailsModel> _SelectedPurchaseDetails = new();
   
    [ObservableProperty]
    private bool _ShowTransactions = false;
    [ObservableProperty]
    private ObservableCollection<SupplierModel> _SupplierList = new();
    [ObservableProperty]
    private decimal _TotalAmountSum;
    [ObservableProperty]
    private int _TotalTransactionCount;
    [ObservableProperty]
    private string _TransactionFilterText = "";
    
    
   

  

    partial void OnDiscountAndTaxOnTotalChanged(bool value)
    {
        if (DiscountAndTaxOnTotal)
        {
            DiscountAndTaxPerItem = false;
            foreach (var item in PurchaseCartList)
            {
                item.DiscountPercent = 0;
                item.TaxPercent = 0;
            }
        }
    }

    partial void OnDiscountAndTaxPerItemChanged(bool value)
    {
        if (DiscountAndTaxPerItem)
        {
            DiscountAndTaxOnTotal = false;
            NewPurchase.DiscountPercent = 0;
            NewPurchase.TaxPercent = 0;
            Calculation();
        }
    }

    partial void OnFilteredTransactionsListChanged(ObservableCollection<NewPurchaseModel> value)
    {
        MakeTransactionsSummary();
    }

    partial void OnProductSearchTermChanged(string value)
    {
        FilterProducts();
    }



    partial void OnPurchaseCartListChanged(ObservableCollection<NewPurchaseDetailsModel> value)
    {
        Calculation();
    }



    partial void OnTransactionFilterTextChanged(string value)
    {
        FilterPurchaseTransactions();
    }

    #endregion Properties

    #region Command Functions

    private bool CanExecuteAddNewPaymentType()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecuteClearCartList()
    {
        return PurchaseCartList.Any();
    }

    private bool CanExecuteJustPrintInvoice(short arg)
    {
        return CurrentUser.ViewReports && SelectedPurchase is not null;
    }

    private bool CanExecuteNewPurchase()
    {
        return CurrentUser.CreateInvoice && PurchaseCartList.Any(); ;
    }

    private bool CanExecuteNewSupplierWindow()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecutePurchaseTransactionDelete()
    {
        return CurrentUser?.DeleteInvoice ?? false && SelectedPurchase is not null;
    }

    private bool CanExecutePurchaseTransactionView()
    {
        return CurrentUser?.ViewReports ?? false && SelectedPurchase is not null;
    }

    private bool CanExecuteSaveNewSupplier()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private void ExecuteAddToPurchaselist()
    {
        AddtoPurchaseList(SelectedProduct);
    }

    private void ExecuteClearCartList()
    {
        PurchaseCartList.Clear();
    }

    

    private void ExecuteClearSelectedSupplier()
    {
        ListSelectedSupplier = null;
    }

    private void ExecuteDecreaseDiscount(NewPurchaseDetailsModel obj)
    {
        if (obj.DiscountPercent > 0)
        {
            obj.DiscountPercent--;
        }
    }

    private void ExecuteDecreaseQuantity(NewPurchaseDetailsModel obj)
    {
        try
        {
            if (PurchaseCartList.Any(cartlist => cartlist.ProductName == obj.ProductName))
            {
                var item = PurchaseCartList.FirstOrDefault(i => i.ProductName == obj.ProductName);
                if (item != null)
                {
                    if (item.Qty == 1)
                    {
                        PurchaseCartList.Remove(PurchaseCartList.FirstOrDefault(i => i.ProductName == obj.ProductName));
                    }
                    else
                    {
                        item.Qty--;
                        OnPropertyChanged(nameof(NewPurchase.SubTotal));
                    }
                }
            }
            else
            {
            }
            SubTotalFunc();
            StopDispatcherIfPurchaseListIsEmpty();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    private void ExecuteDecreaseTax(NewPurchaseDetailsModel obj)
    {
        if (obj?.TaxPercent > 0)
        {
            obj.TaxPercent--;
        }
    }

    private void ExecuteHideView()
    {
        IsNewSupplierViewVisible = false;
    }

    private void ExecuteIncreaseDiscount(NewPurchaseDetailsModel obj)
    {
        if (obj?.DiscountPercent < 99)
        {
            obj.DiscountPercent++;
        }
    }

    private void ExecuteIncreaseQuantity(NewPurchaseDetailsModel obj)
    {
        obj.Qty++;
    }

    private void ExecuteIncreaseTax(NewPurchaseDetailsModel obj)
    {
        if (obj?.TaxPercent < 99)
        {
            obj.TaxPercent++;
        }
    }

    private async Task ExecuteInsertNewPurchase()
    {
        if (SetPurchaseValues())
        {
            if (await _messages.AskUser("Are you sure to save this purchase?"))
            {
                if (await _newPurchaseService.InsertNewPurchaseTransactionAsync(NewPurchase, PurchaseCartList))
                {
                    _messages.ShowSuccessNotification("Transaction Saved.");
                    ExecuteResetAll();
                }
            }
            else
            {
                if (await _messages.AskUser("Reset purchase?"))
                {
                    PurchaseCartList.Clear();
                    ExecuteResetAll();
                }
            }
        }
        else
        {
            _messages.ShowInfoNotification("Please select supplier.");
        }
    }

    private async Task ExecuteJustPrintInvoice(short obj)
    {
        if (SelectedPurchase is not null)
        {
            await PrepareDataForPrinting(SelectedPurchase);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.PurchaseInvoice, false, true, false, obj);
            PrintInvoiceSource.Clear();
        }
    }

    private async Task ExecuteLoadData()
    {
        dt.Tick += dtTicker;
        dt.Interval = TimeSpan.FromSeconds(0.1);
        CurrentUser = CommonProperties.CurrentUser;
        GetCompanyDetails();
        await GetSuppliers();
        await ExecuteRebuildList();
        await GetSuppliersInCombo();
        await GetCashiersInCombo();
        await GetPaymentTypes();
        FilterProducts();
    }

    private async Task ExecutePurchaseTransactionDelete()
    {
        if (SelectedPurchase is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete Transaction 'PUR-{SelectedPurchase.PurchaseID}'?"))
            {
                await _supplierService.DeletePurchaseTransactionAsync(SelectedPurchase);
                MasterTransactionsList.Remove(SelectedPurchase);
                FilteredTransactionsList.Remove(SelectedPurchase);
            }
        }
    }

    private async Task ExecutePurchaseTransactionView()
    {
        if (SelectedPurchase is not null)
        {
            await PrepareDataForPrinting(SelectedPurchase);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.PurchaseInvoice, true, false);
            PrintInvoiceSource.Clear();
            SelectedPurchaseDetails?.Clear();
        }
    }

    private async Task ExecuteRebuildList()
    {
        MasterProductList = await _newSaleService.GetAllProducts();
        FilteredProductList = MasterProductList;
    }

    private async Task ExecuteRefreshPaymentType()
    {
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
    }

    private async Task ExecuteRefreshTransactions(string? obj)
    {
        switch (obj)
        {
            case "Today":
                await GetTodayTransactions();
                break;

            case "Yesterday":
                await GetYesterdayTransactions();
                break;

            default:
                break;
        }
    }

    private void ExecuteRemoveItemFromPurchaseList(NewPurchaseDetailsModel obj)
    {
        try
        {
            if (PurchaseCartList.Any(i => i.ProductName == obj.ProductName))
            {
                var item = PurchaseCartList.FirstOrDefault(i => i.ProductName == obj.ProductName);
                if (item != null)
                {
                    PurchaseCartList.Remove(PurchaseCartList.FirstOrDefault(i => i.ProductName == obj.ProductName));
                }
            }
            else
            {
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
        SubTotalFunc();
        Calculation();
    }

    private void ExecuteResetAll()
    {
        dt.Stop();
        PurchaseCartList.Clear();
        ListSelectedSupplier = null;
        
        DiscountAndTaxOnTotal = true;
       
        NewPurchase = new();
        NewPurchase.PurchaseDate = DateTime.Today;
        SelectedPaymentType = PaymentTypeList.FirstOrDefault(i => i.PaymentType == Settings.Default.DefaultPaymentType);
        CalculatePurchaselistSummary();
    }

    private async Task ExecuteSaveNewSupplier()
    {
        if (!string.IsNullOrWhiteSpace(NewSupplier.SupplierName))
        {
            if (await _supplierService.AddSupplierAsync(NewSupplier))
            {
                IsNewSupplierViewVisible = false;
                await GetSuppliers();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private void ExecuteShowNewSupplierView()
    {
        IsNewSupplierViewVisible = true;
    }
    private async Task ExecuteShowTransactions()
    {
        if (ShowTransactions)
        {
            ShowTransactions = false;
        }
        else
        {
            await Task.Delay(100);
            await GetTodayTransactions();
            ShowTransactions = true;
        }
    }

    #endregion Command Functions

    #region Private Functions
    private void MakeTransactionsSummary()
    {
        if (FilteredTransactionsList is not null)
        {
            TotalTransactionCount = FilteredTransactionsList.Count();
            TotalAmountSum = FilteredTransactionsList.Sum(x => x.GrandTotal);
        }
    }
    public void SubTotalFunc()
    {
        NewPurchase.SubTotal = PurchaseCartList.Sum(a => a.Total);
        Calculation();
    }

    private void AddtoPurchaseList(BaseProductModel obj)
    {
        dt.Start();
        if (obj != null)
        {
            if (PurchaseCartList.Any(i => i.ProductName == obj.Name))
            {
                var item = PurchaseCartList.FirstOrDefault(i => i.ProductName == obj.Name);
                if (item != null)
                {
                    item.Qty++; ;
                }
            }
            else
            {
                PurchaseCartList.Add(new NewPurchaseDetailsModel { Picture = obj.Picture, ProductName = obj.Name, PurchasePrice = obj.PurchasePrice, SalePrice = obj.SalePrice, Qty = 1 });
            }
        }
        Calculation();
    }

    private void CalculatePurchaselistSummary()
    {
        if (PurchaseCartList is not null)
        {
            PurchaselistCount = PurchaseCartList.Count;
            PurchaselistQuantity = PurchaseCartList.Sum(x => x.Qty);
        }
    }

    private void Calculation()
    {
        decimal subTotal = NewPurchase.SubTotal;
        decimal discountPercent = NewPurchase.DiscountPercent;
        decimal taxValue = NewPurchase.TaxPercent;
        //(1000) - ((1000 * 5) / 100))
        decimal priceAfterDiscount = subTotal - (subTotal * discountPercent / 100);
        NewPurchase.DiscountAmount = NewPurchase.SubTotal - priceAfterDiscount;
        decimal priceAfterGST = (priceAfterDiscount + (priceAfterDiscount * taxValue / 100));
        NewPurchase.TaxAmount = priceAfterGST - priceAfterDiscount;
        NewPurchase.GrandTotal = priceAfterGST;
        CalculatePurchaselistSummary();
        StopDispatcherIfPurchaseListIsEmpty();
    }

    private void dtTicker(object sender, EventArgs e) => SubTotalFunc();

    private void FilterProducts()
    {
        FilteredProductList = MasterProductList.Where(i => i.ProductInfo.ToLower().Contains(ProductSearchTerm.ToLower())).ToObservableCollection();
    }

    private void FilterPurchaseTransactions()
    {
        FilteredTransactionsList = MasterTransactionsList.Where(i => i.FilterAble.ToLower().Contains(TransactionFilterText.ToLower())).ToObservableCollection();
    }

    private async Task GetCashiersInCombo()
    {
        ComboCashierList = await _userManagementService.GetUserListAsync();
        ComboCashierList.Insert(0, new UserModel { Username = "All" });
    }

    private void GetCompanyDetails()
    {
        CompanyDetails = CommonProperties.CompanyDetails;
    }

    private async Task GetPaymentTypes()
    {
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
        SelectedPaymentType = PaymentTypeList.FirstOrDefault(i => i.PaymentType == Properties.Settings.Default.DefaultPaymentType);
    }

    private async Task GetSuppliers()
    {
        SupplierList = await _supplierService.GetAllSuppliersAsync();
    }

    private async Task GetSuppliersInCombo()
    {
        ComboSupplierList = await _supplierService.GetAllSuppliersAsync();
        ComboSupplierList.Insert(0, new SupplierModel { SupplierName = "All" });
    }

    private async Task GetTodayTransactions()
    {
        if (CurrentUser?.ViewReports ?? false)
        {
            MasterTransactionsList = await _purchaseReportService.GetTodayPurchaseTransactionsAsync();
            FilteredTransactionsList = MasterTransactionsList;
            FilterPurchaseTransactions();
        }
    }

    private async Task GetYesterdayTransactions()
    {
        if (CurrentUser?.ViewReports ?? false)
        {
            MasterTransactionsList = await _purchaseReportService.GetYesterdayPurchaseTransactionsAsync();
            FilteredTransactionsList = MasterTransactionsList;
            FilterPurchaseTransactions();
        }
    }

    private async Task PrepareDataForPrinting(NewPurchaseModel obj)
    {
        if (SelectedPurchase != null)
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
                       NewPurchaseDetails = await SetPurchaseDetailValuesForPrinting(obj) ?? new ObservableCollection<NewPurchaseDetailsModel>(),
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

    private async Task<ObservableCollection<NewPurchaseDetailsModel>> SetPurchaseDetailValuesForPrinting(NewPurchaseModel obj)
    {
        SelectedPurchaseDetails = await _supplierService.GetTransactionsDetailsBySupplierAsync(obj.PurchaseID);
        ObservableCollection<NewPurchaseDetailsModel> items = new ObservableCollection<NewPurchaseDetailsModel>();
        foreach (var item in SelectedPurchaseDetails ?? new ObservableCollection<NewPurchaseDetailsModel>())
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
        return items;
    }

    private bool SetPurchaseValues()
    {
        NewPurchase.PurchaseBY = CurrentUser.Username;
        NewPurchase.DiscountOnTotal = DiscountAndTaxOnTotal;
        NewPurchase.PaymentType = SelectedPaymentType?.PaymentType;
        NewPurchase.TransactionType = TransactionType.Purchase;
        if (ListSelectedSupplier is not null)
        {
            NewPurchase.SupplierID = ListSelectedSupplier.SupplierID;
            NewPurchase.SupplierName = ListSelectedSupplier.SupplierName;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StopDispatcherIfPurchaseListIsEmpty()
    {
        if (PurchaseCartList.Count == 0)
        {
            dt.Stop();
        }
    }

    #endregion Private Functions
}