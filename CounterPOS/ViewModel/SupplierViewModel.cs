namespace CounterPOS.ViewModel;

public partial class SupplierViewModel : ObservableObject
{
    private readonly ISupplierService _supplierService;
    private readonly IDialogMessages _messages;
    private readonly IEasyPrinting _easyPrinting;

    public SupplierViewModel(ISupplierService supplierService, IDialogMessages messages, IEasyPrinting easyPrinting)
    {
        _supplierService = supplierService;
        _messages = messages;
        _easyPrinting = easyPrinting;
    }

    #region Commands

    public AsyncCommand DeleteSupplierCommand => new AsyncCommand(ExecuteDeleteSupplier, CanExecuteDeleteSupplier);
    public AsyncCommand<short> JustPrintInvoiceCommand => new AsyncCommand<short>(ExecuteJustPrintInvoice, CanExecuteJustPrintInvoice);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public AsyncCommand PurchaseTransactionDeleteCommand => new AsyncCommand(ExecutePurchaseTransactionDelete, CanExecutePurchaseTransactionDelete);
    public AsyncCommand PurchaseTransactionViewCommand => new AsyncCommand(ExecutePurchaseTransactionView, CanExecutePurchaseTransactionView);
    public AsyncCommand RefreshTransactionsListCommand => new AsyncCommand(ExecuteRefreshTransactionsListCommand);
    public AsyncCommand SaveNewSupplierCommand => new AsyncCommand(ExecuteSaveNewSupplier, CanExecuteSaveNewSupplier);
    public AsyncCommand UpdateSupplierCommand => new AsyncCommand(ExecuteUpdateSupplier, CanExecuteUpdateSupplier);
    public DelegateCommand ShowNewSupplierViewCommand => new DelegateCommand(ExecuteShowNewSupplierView, CanExecuteNewSupplierWindow);
    public DelegateCommand ShowEditSupplierViewCommand => new DelegateCommand(ExecuteShowEditSupplierView, CanExecuteUpdateSupplierWindow);
    public DelegateCommand HideViewCommand => new DelegateCommand(ExecuteHideViewCommand);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private bool _IsEditSupplierViewVisible = false;
    [ObservableProperty]
    private bool _IsNewSupplierViewVisible = false;
    [ObservableProperty]
    private bool _IsTransactionsViewVisible = true;


    [ObservableProperty]
    private CompanyModel _CompanyDetails;
    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _FilteredPurchaseListBySupplier;
    [ObservableProperty]
    private ObservableCollection<SupplierModel> _FilteredSupplierList;
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _MasterPurchaseListBySuplier;
    [ObservableProperty]
    private ObservableCollection<SupplierModel> _MasterSupplierList;
    [ObservableProperty]
    private ObservableCollection<NewPurchaseDetailsModel> _NewPurchaseDetailsBySupplier;
    [ObservableProperty]
    private SupplierModel _NewSupplier = new();
    [ObservableProperty]
    private ObservableCollection<NewPurchaseModel> _PrintInvoiceSource = new();
    [ObservableProperty]
    private NewPurchaseModel _SelectedPurchaseBySupplier = new();
    [ObservableProperty]
    private SupplierModel _SelectedSupplier;
    [ObservableProperty]
    private int _SupplierCount;
    [ObservableProperty]
    private string _SupplierFilterText = "";
    [ObservableProperty]
    private string _TransactionFilterText = "";
    [ObservableProperty]

    private int _TransactionsBySupplierCount;
    [ObservableProperty]
    private decimal _TransactionTotalAmount;

   partial void OnFilteredPurchaseListBySupplierChanged(ObservableCollection<NewPurchaseModel> value)
   {
        MakeTransactionSummary();
   }

    partial void OnFilteredSupplierListChanged(ObservableCollection<SupplierModel> value)
    {
        MakeSupplierSummary();
    }


    partial void OnSelectedSupplierChanged(SupplierModel value)
    {
        Task.Run(() => GetPurchaseListBySupplier());
    }


    partial void OnSupplierFilterTextChanged(string value)
    {
        FilterSuppliers();
    }

    partial void OnTransactionFilterTextChanged(string value)
    {
        FilterTransactions();
    }


    


    #endregion Properties

    #region Command Functions

    private void ShowNewSupplierView()
    {
        IsEditSupplierViewVisible = false;
        IsNewSupplierViewVisible = true;
        IsTransactionsViewVisible = false;
    }

    private void ShowEditSupplierView()
    {
        IsEditSupplierViewVisible = true;
        IsNewSupplierViewVisible = false;
        IsTransactionsViewVisible = false;
    }

    private void ShowTransactionsView()
    {
        IsEditSupplierViewVisible = false;
        IsNewSupplierViewVisible = false;
        IsTransactionsViewVisible = true;
    }

    private bool CanExecuteDeleteSupplier()
    {
        return CurrentUser?.DeleteAccount ?? false && SelectedSupplier is not null;
    }

    private bool CanExecuteJustPrintInvoice(short arg)
    {
        return CurrentUser.ViewReports && SelectedPurchaseBySupplier is not null;
    }

    private bool CanExecuteNewSupplierWindow()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecutePurchaseTransactionDelete()
    {
        return CurrentUser?.DeleteInvoice ?? false;
    }

    private bool CanExecutePurchaseTransactionView()
    {
        return CurrentUser?.ViewReports ?? false;
    }

    private bool CanExecuteSaveNewSupplier()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecuteUpdateSupplier()
    {
        return CurrentUser?.EditAccount ?? false;
    }

    private bool CanExecuteUpdateSupplierWindow()
    {
        return CurrentUser?.EditAccount ?? false && SelectedSupplier is not null;
    }

    private async Task ExecuteDeleteSupplier()
    {
        if (SelectedSupplier is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete '{SelectedSupplier.SupplierName}'?"))
            {
                if (await _supplierService.DeleteSupplierAsync(SelectedSupplier))
                {
                    MasterSupplierList.Remove(SelectedSupplier);
                    FilteredSupplierList.Remove(SelectedSupplier);
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteJustPrintInvoice(short obj)
    {
        if (SelectedPurchaseBySupplier is not null)
        {
            await PrepareDataForPrinting(SelectedPurchaseBySupplier);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.PurchaseInvoice, false, true, false, obj);
            PrintInvoiceSource.Clear();
            NewPurchaseDetailsBySupplier.Clear();
        }
    }

    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        await GetSuppliers();
    }

    private async Task ExecutePurchaseTransactionDelete()
    {
        if (SelectedPurchaseBySupplier is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete Transaction 'PUR-{SelectedPurchaseBySupplier.PurchaseID}'?"))
            {
                if (await _supplierService.DeletePurchaseTransactionAsync(SelectedPurchaseBySupplier))
                {
                    MasterPurchaseListBySuplier.Remove(SelectedPurchaseBySupplier);
                    FilteredPurchaseListBySupplier.Remove(SelectedPurchaseBySupplier);
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecutePurchaseTransactionView()
    {
        if (SelectedPurchaseBySupplier is not null)
        {
            await PrepareDataForPrinting(SelectedPurchaseBySupplier);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.PurchaseInvoice, true, false);
            PrintInvoiceSource.Clear();
            NewPurchaseDetailsBySupplier.Clear();
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteRefreshTransactionsListCommand()
    {
        await GetPurchaseListBySupplier();
    }

    private async Task ExecuteSaveNewSupplier()
    {
        if (!string.IsNullOrWhiteSpace(NewSupplier.SupplierName))
        {
            if (await _supplierService.AddSupplierAsync(NewSupplier))
            {
                int maxID = _supplierService.GetLastSupplierID();
                NewSupplier.SupplierID = maxID;
                MasterSupplierList.Add(NewSupplier);
                FilteredSupplierList.Add(NewSupplier);

                NewSupplier = new();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private async Task ExecuteUpdateSupplier()
    {
        if (!string.IsNullOrWhiteSpace(SelectedSupplier.SupplierName))
        {
            await _supplierService.UpdateSupplierAsync(SelectedSupplier);
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private async Task GetSuppliers()
    {
        MasterSupplierList = await _supplierService.GetAllSuppliersAsync();
        FilteredSupplierList = MasterSupplierList;
        FilterSuppliers();
    }

    #endregion Command Functions

    #region Private Functions

    private void ExecuteShowEditSupplierView()
    {
        if (SelectedSupplier is not null)
        {
            ShowEditSupplierView();
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private void ExecuteShowNewSupplierView()
    {
        NewSupplier = new();
        ShowNewSupplierView();
    }

    private void ExecuteHideViewCommand()
    {
        ShowTransactionsView();
    }

    private void FilterSuppliers()
    {
        FilteredSupplierList = MasterSupplierList.Where(i => i.SupplierFull.ToLower().Contains(SupplierFilterText.ToLower())).ToObservableCollection();
    }

    private void FilterTransactions()
    {
        FilteredPurchaseListBySupplier = MasterPurchaseListBySuplier.Where(i => i.FilterAble.ToLower().Contains(TransactionFilterText.ToLower())).ToObservableCollection();
    }

    private async Task GetPurchaseListBySupplier()
    {
        if (CurrentUser?.ViewReports ?? false)
        {
            if (IsTransactionsViewVisible)
            {
                if (SelectedSupplier is not null)
                {
                    MasterPurchaseListBySuplier = await _supplierService.GetPurchaseBySupplierAsync(SelectedSupplier.SupplierID);
                    FilteredPurchaseListBySupplier = MasterPurchaseListBySuplier;
                }
                else
                {
                    MasterPurchaseListBySuplier?.Clear();
                    FilteredPurchaseListBySupplier = MasterPurchaseListBySuplier;
                }
                FilterTransactions();
            }
        }
    }

    private void MakeSupplierSummary()
    {
        if (FilteredSupplierList is not null)
        {
            SupplierCount = FilteredSupplierList.Count();
        }
    }

    private void MakeTransactionSummary()
    {
        if (FilteredPurchaseListBySupplier is not null)
        {
            TransactionsBySupplierCount = FilteredPurchaseListBySupplier.Count();
            TransactionTotalAmount = FilteredPurchaseListBySupplier.Sum(x => x.GrandTotal);
        }
    }

    private async Task PrepareDataForPrinting(NewPurchaseModel obj)
    {
        if (SelectedPurchaseBySupplier != null)
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
        NewPurchaseDetailsBySupplier = await _supplierService.GetTransactionsDetailsBySupplierAsync(obj.PurchaseID);
        ObservableCollection<NewPurchaseDetailsModel> items = new ObservableCollection<NewPurchaseDetailsModel>();
        foreach (var item in NewPurchaseDetailsBySupplier)
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
                TaxAmount = item.TaxAmount,
            });
        }
        return items;
    }

    #endregion Private Functions
}