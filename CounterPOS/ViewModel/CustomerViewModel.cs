using System.Collections.ObjectModel;

namespace CounterPOS.ViewModel;

public partial class CustomerViewModel : ObservableObject
{
    private readonly ICustomerService _customerService;
    private readonly IEasyPrinting _easyPrinting;
    private readonly IDialogMessages _messages;
    private readonly INewSaleService _newSaleService;
    private readonly ISaleReportService _saleReportService;

    public CustomerViewModel(ICustomerService customerService, INewSaleService newSaleService, ISaleReportService saleReportService, IDialogMessages messages, IEasyPrinting easyPrinting)
    {
        _customerService = customerService;
        _newSaleService = newSaleService;
        _saleReportService = saleReportService;
        _messages = messages;
        _easyPrinting = easyPrinting;
    }

    #region Commands

    public AsyncCommand AddCustomerCommand => new AsyncCommand(ExecuteAddCustomer, CanExecuteAddCustomer);
    public AsyncCommand DeleteCustomerCommand => new AsyncCommand(ExecuteDeleteCustomer, CanExecuteDeleteCustomer);
    public AsyncCommand EditCustomerCommand => new AsyncCommand(ExecuteEditCustomer, CanExecuteEditCustomer);
    public DelegateCommand HideViewCommand => new DelegateCommand(ExecuteHideViewCommand);
    public AsyncCommand<short> JustPrintInvoiceCommand => new AsyncCommand<short>(ExecuteJustPrintInvoice, CanExecuteJustPrintInvoice);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public AsyncCommand RefreshTransactionsCommand => new AsyncCommand(ExecuteRefreshTransactions);
    public DelegateCommand ShowEditCustomerViewCommand => new DelegateCommand(ExecuteShowEditCustomerView, CanExecuteEditCustomerWindow);
    public DelegateCommand ShowNewCustomerViewCommand => new DelegateCommand(ExecuteShowNewCustomerView, CanExecuteAddCustomerWindow);
    public AsyncCommand TransactionDeleteCommand => new AsyncCommand(ExecuteDeleteTransaction, CanExecuteDeleteTransaction);
    public AsyncCommand TransactionViewCommand => new AsyncCommand(ExecuteViewTransaction, CanExecuteViewTransaction);

    #endregion Commands

    #region Properties

    [ObservableProperty]
    private int _BlacklistCustomerCount = 0;

    [ObservableProperty]
    private CompanyModel _CompanyDetails = new();

    [ObservableProperty]
    private UserModel _CurrentUser = new();

    [ObservableProperty]
    private int _CustomerCount = 0;

    [ObservableProperty]
    private string _CustomerFilterText = "";

    [ObservableProperty]
    private decimal _DiscountGiven;

    [ObservableProperty]
    private int _FBRInvoicesCount = 0;

    [ObservableProperty]
    private ObservableCollection<CustomerModel> _FilteredCustomerList = new();

    [ObservableProperty]
    private ObservableCollection<TransactionModel> _FilterTransactionsList = new();

    [ObservableProperty]
    private ObservableCollection<CustomerModel> _MasterCustomerList;

    [ObservableProperty]
    private ObservableCollection<TransactionModel> _MasterTransactionsList = new();

    [ObservableProperty]
    private CustomerModel _NewCustomer = new();

    [ObservableProperty]
    private ObservableCollection<TransactionModel> _PrintInvoiceSource = new();

    [ObservableProperty]
    private CustomerModel _SelectedCustomer;

    [ObservableProperty]
    private TransactionModel _SelectedTransactionByCustomer;

    [ObservableProperty]
    private bool _ShowBlackListCustomersOnly;

    [ObservableProperty]
    private decimal _TotalAmmount;

    [ObservableProperty]
    private decimal _TotalAmountSum = 0;

    [ObservableProperty]
    private int _TotalTransactionCount = 0;

    [ObservableProperty]
    private int _TransactionCount;

    [ObservableProperty]
    private ObservableCollection<TransactionDetailsModel> _TransactionDetailsByCustomer = new();

    [ObservableProperty]
    private string _TransactionFilterText = "";

    [ObservableProperty]
    private bool _IsEditCustomerViewVisible = false;

    [ObservableProperty]
    private bool _IsNewCustomerViewVisible = false;

    [ObservableProperty]
    private bool _IsTransactionsViewVisible = true;

    

    private bool _ShowBlacklistOnly = false;

    public bool ShowBlacklistOnly
    {
        get { return _ShowBlacklistOnly; }
        set { _ShowBlacklistOnly = value; OnPropertyChanged(); ToggleBlacklistCustomers(value); }
    }   


    partial void OnCustomerFilterTextChanged(string value)
    {
        FilterCustomers();
    }

     partial void OnFilteredCustomerListChanged(ObservableCollection<CustomerModel> value)
    {
        MakeCustomerSummary();
    }

     partial void OnFilterTransactionsListChanged(ObservableCollection<TransactionModel> value)
    {
        MakeTransactionsSummary();
    }

     partial void OnSelectedCustomerChanged(CustomerModel value)
    {
        Task.Run(async () => await ExecuteRefreshTransactions());
    }

     partial void OnTransactionFilterTextChanged(string value)
    {
        FilterTransactions();
    }

    

    #endregion Properties

    #region Command Functions

    private bool CanExecuteAddCustomer()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecuteAddCustomerWindow()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecuteDeleteCustomer()
    {
        return CurrentUser?.DeleteAccount ?? false && SelectedCustomer is not null;
    }

    private bool CanExecuteDeleteTransaction()
    {
        //return false;
        return SelectedTransactionByCustomer is not null && CurrentUser.DeleteInvoice;
    }

    private bool CanExecuteEditCustomer()
    {
        return CurrentUser?.EditAccount ?? false && SelectedCustomer is not null;
    }

    private bool CanExecuteEditCustomerWindow()
    {
        return CurrentUser?.EditAccount ?? false && SelectedCustomer is not null;
    }

    private bool CanExecuteJustPrintInvoice(short arg)
    {
        return CurrentUser.ViewReports && SelectedTransactionByCustomer is not null;
    }

    private bool CanExecuteViewTransaction()
    {
        return CurrentUser?.ViewReports ?? false && SelectedTransactionByCustomer is not null;
    }

    private async Task ExecuteAddCustomer()
    {
        if (!string.IsNullOrWhiteSpace(NewCustomer?.Name))
        {
            if (await _customerService.AddCustomerAsync(NewCustomer))
            {
                var lastID = _customerService.GetLastCustomerID();
                NewCustomer.Id = lastID;
                MasterCustomerList.Add(NewCustomer);
                FilteredCustomerList.Add(NewCustomer);
                NewCustomer = new();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private async Task ExecuteDeleteCustomer()
    {
        if (SelectedCustomer is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete '{SelectedCustomer.Name}'?"))
            {
                if (await _customerService.DeleteCustomerAsync(SelectedCustomer))
                {
                    MasterCustomerList.Remove(SelectedCustomer);
                    FilteredCustomerList.Remove(SelectedCustomer);
                    //await RefreshCustomer();
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteDeleteTransaction()
    {
        if (SelectedTransactionByCustomer is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete Transaction 'INV-{SelectedTransactionByCustomer.TransactionID}'?"))
            {
                if (await _newSaleService.DeleteAnSaveTransactionAsync(SelectedTransactionByCustomer))
                {
                    //if (SelectedCustomer is not null)
                    //{
                    MasterTransactionsList.Remove(SelectedTransactionByCustomer);
                    FilterTransactionsList.Remove(SelectedTransactionByCustomer);
                    //if (string.IsNullOrEmpty(SelectedCustomer.ID.ToString()))
                    //{
                    //    TransactionDetailsByCustomer = await _customerService.GetTransactionsDetailsByCustomerAsync(SelectedTransactionByCustomer?.TransactionID);
                    //}
                    //}
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteEditCustomer()
    {
        if (!string.IsNullOrWhiteSpace(SelectedCustomer?.Name))
        {
            await _customerService.UpdateCustomerAsync(SelectedCustomer);
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private void ExecuteHideViewCommand()
    {
        ShowTransactionsView();
    }

    private async Task ExecuteJustPrintInvoice(short obj)
    {
        if (SelectedTransactionByCustomer is not null)
        {
            await PrepareDataForPrinting(SelectedTransactionByCustomer);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, false, true, SelectedTransactionByCustomer.IsPaid, obj);
            PrintInvoiceSource.Clear();
        }
    }

    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        await RefreshCustomer();
    }

    private async Task ExecuteRefreshTransactions()
    {
        if (CurrentUser?.ViewReports ?? false)
        {
            if (IsTransactionsViewVisible)
            {
                if (SelectedCustomer is not null && !string.IsNullOrEmpty(SelectedCustomer.Id.ToString()))
                {
                    MasterTransactionsList = await _customerService.GetTransactionsByCustomerAsync(SelectedCustomer.Id);
                    FilterTransactionsList = MasterTransactionsList;
                }
                else
                {
                    MasterTransactionsList.Clear();
                    FilterTransactionsList = MasterTransactionsList;
                }
                FilterTransactions();
            }
        }
    }

    private void ExecuteShowEditCustomerView()
    {
        if (SelectedCustomer is not null)
        {
            ShowEditCustomerView();
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private void ExecuteShowNewCustomerView()
    {
        NewCustomer = new();
        ShowNewCustomerView();
    }

    private async Task ExecuteViewTransaction()
    {
        if (SelectedTransactionByCustomer is not null)
        {
            await PrepareDataForPrinting(SelectedTransactionByCustomer);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, true, false, SelectedTransactionByCustomer.IsPaid);
            PrintInvoiceSource.Clear();
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private void ShowEditCustomerView()
    {
        IsEditCustomerViewVisible = true;
        IsNewCustomerViewVisible = false;
        IsTransactionsViewVisible = false;
    }

    private void ShowNewCustomerView()
    {
        IsEditCustomerViewVisible = false;
        IsNewCustomerViewVisible = true;
        IsTransactionsViewVisible = false;
    }

    private void ShowTransactionsView()
    {
        IsEditCustomerViewVisible = false;
        IsNewCustomerViewVisible = false;
        IsTransactionsViewVisible = true;
    }

    #endregion Command Functions

    #region Private Functions

    private void ToggleBlacklistCustomers(bool value)
    {
        if (value)
        {
            FilteredCustomerList = MasterCustomerList.Where(i => i.Blacklist == true).ToObservableCollection();
        }
        else
        {
            CustomerFilterText = string.Empty;
            FilteredCustomerList = MasterCustomerList;
        }
        
    }   

    private void FilterCustomers()
    {
        FilteredCustomerList = MasterCustomerList.Where(i => i.CustomerFull.ToLower().Contains(CustomerFilterText.ToLower())).ToObservableCollection();
    }

    private void FilterTransactions()
    {
        FilterTransactionsList = MasterTransactionsList.Where(i => i.FIlterAble.ToLower().Contains(TransactionFilterText.ToLower())).ToObservableCollection();
    }

    private async Task<ObservableCollection<TransactionDetailsModel>> GetTransactionDetailValuesForPrinting(TransactionModel obj)
    {
        ObservableCollection<TransactionDetailsModel> items = new();
        if (obj is not null)
        {
            var transactionDetailsByCustomer = await _saleReportService.GetTransactionsDetailsAsync(obj.TransactionID);
            items = new ObservableCollection<TransactionDetailsModel>(
                transactionDetailsByCustomer.Select(item => new TransactionDetailsModel
                {
                    TransactionID = item.TransactionID,
                    Name = item.Name,
                    Qty = item.Qty,
                    Price = item.Price,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    Total = item.Total,
                }));

            
        }

        return items;
    }

    private void MakeCustomerSummary()
    {
        if (FilteredCustomerList is not null)
        {
            CustomerCount = FilteredCustomerList.Count();
            BlacklistCustomerCount = FilteredCustomerList.Count(x => x.Blacklist);
        }
    }

    private void MakeTransactionsSummary()
    {
        if (FilterTransactionsList is not null)
        {
            TotalTransactionCount = FilterTransactionsList.Count();
            FBRInvoicesCount = FilterTransactionsList.Count(x => x.IsFBRInvoice);
            TotalAmountSum = FilterTransactionsList.Sum(x => x.GrandTotal);
        }
    }

    private async Task PrepareDataForPrinting(TransactionModel? obj)
    {
        if (obj is not null)
        {
            var transactionDetail = await GetTransactionDetailValuesForPrinting(obj);
            var companyDetails = SetCompanyDetails();
            var fbrResponse = await SetFBRResponse(obj) ?? new FBRResponseModel();

            PrintInvoiceSource?.Add(new TransactionModel
            {
                TransactionID = obj.TransactionID,
                Narration = obj.Narration,
                InvoiceDate = obj.InvoiceDate,
                InvoiceTime = obj.InvoiceTime,
                SubTotal = obj.SubTotal,
                DiscountValue = obj.DiscountValue,
                DiscountPrice = obj.DiscountPrice,
                GSTValue = obj.GSTValue,
                GSTPrice = obj.GSTPrice,
                Cash = obj.Cash,
                Change = obj.Change,
                GrandTotal = obj.GrandTotal,
                AdditionalCharges = obj.AdditionalCharges,
                IsPaid = obj.IsPaid,
                Cashier = obj.Cashier,
                TransactionDetail = transactionDetail,
                CompanyDetails = companyDetails,
                FBRResponse = fbrResponse,
                FBRImageSource = @"Resources\FbrPosImage.png",
                IsFBRInvoice = obj.IsFBRInvoice,
                DiscountOnTotal = obj.DiscountOnTotal
            });
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task RefreshCustomer()
    {
        MasterCustomerList = await _customerService.GetCustomersAsync();
        FilteredCustomerList = MasterCustomerList;
        FilterCustomers();
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

    private async Task<FBRResponseModel?> SetFBRResponse(TransactionModel obj)
    {
        if (obj.IsFBRInvoice)
        {
            return await _saleReportService.GetFBRResponseAsync(obj.TransactionID) ?? new FBRResponseModel();
        }
        else
        {
            FBRResponseModel fBRResponseModel = new FBRResponseModel();
            return fBRResponseModel;
        }
    }

    #endregion Private Functions
}