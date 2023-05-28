namespace CounterPOS.ViewModel;

public partial class ItemReportViewModel : ObservableObject
{
    private readonly IItemReportService _itemReportService;
    private readonly IDialogMessages _messages;
    private readonly IEasyPrinting _easyPrinting;
    public ItemReportViewModel(IItemReportService itemReportService, IDialogMessages messages, IEasyPrinting easyPrinting)
    {
        _itemReportService = itemReportService;
        _messages = messages;
        _easyPrinting = easyPrinting;
        CurrentUser = CommonProperties.CurrentUser;
    }

    #region Commands

    public DelegateCommand ClearItemsReportCommand => new DelegateCommand(ExecuteClearItemsReport);
    public AsyncCommand GenerateItemReportCommand => new AsyncCommand(ExecuteGenerateItemReport, CanExecuteGenerateItemReport);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData, CanExecuteGenerateItemReport);
    public DelegateCommand PrintItemReportCommand => new DelegateCommand(ExecutePrintItemReport, CanExecutePrintItemReport);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private ObservableCollection<ItemReportModel> _FilteredItemReportList = new();
    [ObservableProperty]
    private DateTime _FromDate = DateTime.Now;
    [ObservableProperty]
    private string _ItemSearchTerm = "";
    [ObservableProperty]
    private ObservableCollection<ItemReportModel> _MasterItemReportList = new();
    [ObservableProperty]
    private DateTime _ToDate = DateTime.Now;
    [ObservableProperty]
    private decimal _TotalAmount;

    partial void OnFilteredItemReportListChanged(ObservableCollection<ItemReportModel> value)
    {
        GenerateItemsReportSummary();
    }

    partial void OnItemSearchTermChanged(string value)
    {
        FilterItems();
    }


    #endregion Properties

    #region Command Functions

    private bool CanExecuteGenerateItemReport()
    {
        return CurrentUser.ViewReports;
    }

    private bool CanExecutePrintItemReport()
    {
        return FilteredItemReportList.Count > 0;
    }

    private void ExecuteClearItemsReport()
    {
        
        MasterItemReportList.Clear();
        FilterItems();
    }

    private async Task ExecuteGenerateItemReport()
    {
       
        MasterItemReportList = await _itemReportService.GetItemReportAsync(FromDate, ToDate);

        FilterItems();
    }

    private async Task ExecuteLoadData()
    {
        await ExecuteGenerateItemReport();
    }

    private void ExecutePrintItemReport()
    {
        try
        {
            _easyPrinting.PrintEasily(FilteredItemReportList, InvoiceType.ItemReportInvoice, false, true, false, 1);
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    #endregion Command Functions

    #region Private Functions

    private void FilterItems()
    {
        FilteredItemReportList = MasterItemReportList.Where(i => i.Name.ToLower().Contains(ItemSearchTerm.ToLower())).ToObservableCollection();
    }

    private void GenerateItemsReportSummary()
    {
        if (FilteredItemReportList is not null)
        {
            TotalAmount = FilteredItemReportList.Sum(i => i.Total);
        }
    }

    #endregion Private Functions
}