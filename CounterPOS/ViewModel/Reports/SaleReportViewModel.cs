namespace CounterPOS.ViewModel;

public partial class SaleReportViewModel : ObservableObject
{
    private readonly ISaleReportService _saleReportService;
    private readonly INewSaleService _newSaleService;
    private readonly IPaymentTypeService _paymentTypeService;
    private readonly IDialogMessages _messages;
    private readonly IEasyPrinting _easyPrinting;
    public SaleReportViewModel(ISaleReportService saleReportService, INewSaleService newSaleService, IPaymentTypeService paymentTypeService, IDialogMessages messages, IEasyPrinting easyPrinting)
    {
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        _saleReportService = saleReportService;
        _newSaleService = newSaleService;
        _paymentTypeService = paymentTypeService;
        _messages = messages;
        _easyPrinting = easyPrinting;
    }

    #region Commands

    public AsyncCommand GetFilteredSaleTransactions => new AsyncCommand(ExecuteGetFilteredSaleTransactions, CanExecuteGetTransactions);
    public AsyncCommand<short> JustPrintInvoiceCommand => new AsyncCommand<short>(ExecuteJustPrintInvoice, CanExecuteJustPrintInvoice);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData, CanExecuteGetTransactions);
    public AsyncCommand ShowAllSaleCommand => new AsyncCommand(ExecuteShowAllSaleCommand, CanExecuteGetTransactions);

    public AsyncCommand TransactionDeleteCommand => new AsyncCommand(ExecuteDeletetransaction, CanExecuteDeleteTransaction);
    public AsyncCommand TransactionViewCommand => new AsyncCommand(ExecuteViewTransaction, CanExecuteTransactionView);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private ObservableCollection<UserModel> _CashierList = new();
    [ObservableProperty]
    private CompanyModel _CompanyDetails = new();
    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private decimal _DiscountGiven;
    [ObservableProperty]
    private int _FBRInvoicesCount;
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _FilteredTransactionsList = new();
    [ObservableProperty]
    private DateTime _FromDate = DateTime.Now.Date;
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _MasterTransactionsList = new();
    [ObservableProperty]
    private ObservableCollection<PaymentTypeModel> _PaymentTypeList = new();
    [ObservableProperty]
    private ObservableCollection<TransactionModel> _PrintInvoiceSource = new();
    [ObservableProperty]
    private UserModel _SelectedCashierUsername = new();
    [ObservableProperty]
    private PaymentTypeModel _SelectedPaymentType = new();
    [ObservableProperty]
    private TransactionModel _SelectedTransaction = new();
    [ObservableProperty]
    private decimal _SubTotal;
    [ObservableProperty]
    private DateTime _ToDate = DateTime.Now.Date;
    [ObservableProperty]
    private decimal _TotalAmmount;
    [ObservableProperty]
    private decimal _TotalAmountSum;
    [ObservableProperty]
    private int _TotalTransactionCount;
    [ObservableProperty]
    private int _TransactionCount;
    [ObservableProperty]
    private ObservableCollection<TransactionDetailsModel> _TransactionDetails = new();
    [ObservableProperty]
    private string _TransactionFilterText = "";

  
    partial void OnFilteredTransactionsListChanged(ObservableCollection<TransactionModel> value)
    {
        MakeTransactionsSummary();
    }


    partial void OnTransactionFilterTextChanged(string value)
    {
        FilterTransactions();
    }

    #endregion Properties

    #region Command Functions

    private bool CanExecuteDeleteTransaction()
    {
        return CurrentUser?.DeleteInvoice ?? false;
    }

    private bool CanExecuteGetTransactions()
    {
        return CurrentUser?.ViewReports ?? false;
    }

    private bool CanExecuteJustPrintInvoice(short arg)
    {
        return CurrentUser.ViewReports && SelectedTransaction is not null;
    }

    private bool CanExecuteTransactionView()
    {
        return CurrentUser?.ViewReports ?? false;
    }

    private async Task ExecuteDeletetransaction()
    {
        if (SelectedTransaction is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete Transaction 'INV-{SelectedTransaction.TransactionID}'?"))
            {
                await _newSaleService.DeleteAnSaveTransactionAsync(SelectedTransaction);
                MasterTransactionsList.Remove(SelectedTransaction);
                FilteredTransactionsList.Remove(SelectedTransaction);
                await ExecuteGetFilteredSaleTransactions();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteGetFilteredSaleTransactions()
    {
        if (CurrentUser?.ViewReports ?? false)
        {
            MasterTransactionsList = await _saleReportService.GetTransactionsBetweenDatesWithPaymentTypeAndCashierAsync(FromDate, ToDate, SelectedPaymentType.PaymentType, SelectedCashierUsername.Username);
            FilterTransactions();
        }
    }

    private async Task ExecuteJustPrintInvoice(short obj)
    {
        if (SelectedTransaction is not null)
        {
            await PrepareDataForPrinting(SelectedTransaction);
            _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, false, true, SelectedTransaction.IsPaid, obj);
            PrintInvoiceSource.Clear();
        }
    }

    private async Task ExecuteLoadData()
    {
        PaymentTypeList = await _paymentTypeService.GetPaymentTypeListAsync();
        CashierList = await _saleReportService.GetCashierListAsync();

        SelectedCashierUsername = CashierList.FirstOrDefault();
        SelectedPaymentType = PaymentTypeList.FirstOrDefault();

        await ExecuteGetFilteredSaleTransactions();
    }

    private async Task ExecuteShowAllSaleCommand()
    {
        MasterTransactionsList = await _saleReportService.GetAllTransactionsBetweenTwoDatesAsync(FromDate, ToDate);
        FilterTransactions();
    }

    private async Task ExecuteViewTransaction()
    {
        await PrepareDataForPrinting(SelectedTransaction);
        _easyPrinting.PrintEasily(PrintInvoiceSource, InvoiceType.SaleInvoice, true, false, SelectedTransaction?.IsPaid ?? false);
        PrintInvoiceSource?.Clear();
    }

    #endregion Command Functions

    #region Private Functions

    private void FilterTransactions()
    {
        Application.Current.Dispatcher.BeginInvoke
          (new Action(() => FilteredTransactionsList = MasterTransactionsList.Where(i => i.FIlterAble.ToLower().Contains(TransactionFilterText.ToLower())).ToObservableCollection()
      ));
    }

    private void MakeTransactionsSummary()
    {
        if (FilteredTransactionsList is not null)
        {
            TotalTransactionCount = FilteredTransactionsList.Count();
            FBRInvoicesCount = FilteredTransactionsList.Count(x => x.IsFBRInvoice);
            TotalAmountSum = FilteredTransactionsList.Sum(x => x.GrandTotal);
        }
    }

    private async Task PrepareDataForPrinting(TransactionModel obj)
    {
        var fbrResponse = await SetFBRResponse(obj);

        PrintInvoiceSource.Add
                    (new TransactionModel
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
                        AdditionalCharges = obj.AdditionalCharges,
                        GrandTotal = obj.GrandTotal,
                        IsPaid = obj.IsPaid,
                        Cashier = obj.Cashier,
                        TransactionDetail = await SetTransactionDetailValuesForPrinting(obj),
                        CompanyDetails = SetCompanyDetails(),
                        FBRResponse = fbrResponse,
                        FBRImageSource = @"Resources\FbrPosImage.png",
                        IsFBRInvoice = obj.IsFBRInvoice,
                        DiscountOnTotal = obj.DiscountOnTotal
                    });
    }

    private ObservableCollection<CompanyModel> SetCompanyDetails()
    {
        ObservableCollection<CompanyModel> companyModel = new ObservableCollection<CompanyModel>();
        companyModel.Add
            (new CompanyModel
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
            });

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

    private async Task<ObservableCollection<TransactionDetailsModel>> SetTransactionDetailValuesForPrinting(TransactionModel obj)
    {
        ObservableCollection<TransactionDetailsModel> items = new();
        if (obj is not null)
        {
            TransactionDetails = await _saleReportService.GetTransactionsDetailsAsync(obj.TransactionID);
            
            foreach (var item in TransactionDetails)
            {
                items.Add(new TransactionDetailsModel
                {
                    TransactionID = item.TransactionID,
                    Name = item.Name,
                    Qty = item.Qty,
                    Price = item.Price,
                    Discount = item.Discount,
                    Tax = item.Tax,
                    Total = item.Total,
                });
            }
        }

        return items;
    }

    #endregion Private Functions
}