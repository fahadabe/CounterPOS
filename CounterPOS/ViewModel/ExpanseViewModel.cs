namespace CounterPOS.ViewModel;

public partial class ExpanseViewModel : ObservableObject
{
    private readonly IExpanseService _expanseService;
    private readonly IDialogMessages _messages;
    public ExpanseViewModel(IExpanseService expanseService, IDialogMessages messages)
    {
        _expanseService = expanseService;
        _messages = messages;
    }
    #region Commands

    public AsyncCommand<Window> AddExpanseCommand => new AsyncCommand<Window>(ExecuteAddExpanse, CanExecuteAddExpanse);
    public AsyncCommand DeleteExpanseCommand => new AsyncCommand(ExecuteDeleteExpanse, CanExecuteDeleteExpanse);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public DelegateCommand NewExpanseCategoryWindowCommand => new DelegateCommand(ExecuteNewExpanseCategoryWindow, CanExecuteNewExpanseCategoryWindow);

    public AsyncCommand<Window> UpdateExpanseCommand => new AsyncCommand<Window>(ExecuteUpdateExpanse, CanExecuteUpdateExpanse);
    
    public DelegateCommand ShowNewExpanseViewCommand => new DelegateCommand(ExecuteShowNewExpanseView, CanExecuteAddExpanseWindow);
    public DelegateCommand ShowEditExpanseViewCommand => new DelegateCommand(ExecuteShowEditExpanseView, CanExecuteUpdateExpanseWindow);
    public DelegateCommand HideViewCommand => new DelegateCommand(ExecuteHideViewCommand);







    #endregion Commands

    #region Properties
    [ObservableProperty]
    private CompanyModel _CompanyDetails = new();

    [ObservableProperty]
    private int _ExpanseCount;

    [ObservableProperty]
    private string _ExpanseFilterText = "";

    [ObservableProperty]
    private decimal _ExpanseTotalAmount;

    [ObservableProperty]
    private ObservableCollection<ExpanseModel> _FilteredExpanseList;

    [ObservableProperty]
    private ObservableCollection<ExpanseModel> _MasterExpanseList;

    [ObservableProperty]
    private ExpanseModel _NewExpanse = new();

    [ObservableProperty]
    private ExpanseModel _SelectedExpanse;

    [ObservableProperty]
    private ExpanseCategoryModel _SelectedExpanseCategory = new();

    [ObservableProperty]
    private bool _IsEditExpanseViewVisible = false;

    [ObservableProperty]
    private bool _IsNewExpanseViewVisible = false;

    [ObservableProperty]
    private UserModel _CurrentUser = new();

    [ObservableProperty]
    private ObservableCollection<ExpanseCategoryModel> _ExpanseCategoryList;

    partial void OnExpanseFilterTextChanged(string value)
    {
        FilterExpanse();
    }


    partial void OnFilteredExpanseListChanged(ObservableCollection<ExpanseModel> value)
    {
        MakeExpanseSummary();
    }


    

    

    

    #endregion Properties

    #region Command Functions
    private void ExecuteShowEditExpanseView()
    {
        if (SelectedExpanse is not null)
        {
            ShowEditExpanseView();
             GetExpanseCategories();
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }
    private void ExecuteShowNewExpanseView()
    {
        NewExpanse = new();
        ShowNewExpanseView();
         GetExpanseCategories();
    }

    private void ExecuteHideViewCommand()
    {
        ShowTransactionsView();
    }

    private void ShowNewExpanseView()
    {
        IsEditExpanseViewVisible = false;
        IsNewExpanseViewVisible = true;
    }

    private void ShowEditExpanseView()
    {
        IsEditExpanseViewVisible = true;
        IsNewExpanseViewVisible = false;
    }

    private void ShowTransactionsView()
    {
        IsEditExpanseViewVisible = false;
        IsNewExpanseViewVisible = false;
    }
    private bool CanExecuteAddExpanse(Window win)
    {
        return CurrentUser?.AddExpanse ?? false;
    }

    private bool CanExecuteAddExpanseWindow()
    {
        return CurrentUser?.AddExpanse ?? false;
    }

    private bool CanExecuteDeleteExpanse()
    {
        return CurrentUser?.DeleteExpanse ?? false && SelectedExpanse is not null;
    }

    private bool CanExecuteDeleteExpanseCategory()
    {
        return CurrentUser?.DeleteAccount ?? false && SelectedExpanseCategory is not null;
    }

    private bool CanExecuteNewExpanseCategoryWindow()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private bool CanExecuteUpdateExpanse(Window win)
    {
        return CurrentUser?.EditExpanse ?? false && SelectedExpanse is not null;
    }

    private bool CanExecuteUpdateExpanseWindow()
    {
        return CurrentUser?.EditExpanse ?? false && SelectedExpanse is not null;
    }

    private async Task ExecuteAddExpanse(Window win)
    {
        if (!string.IsNullOrWhiteSpace(NewExpanse.Amount.ToString()) && !string.IsNullOrWhiteSpace(NewExpanse.ExpanseCategory) && !string.IsNullOrWhiteSpace(NewExpanse.ExpansesDescription))
        {
            if (await _expanseService.AddExpanseAsync(NewExpanse))
            {
                int maxID = _expanseService.GetLastExpanseID();
                NewExpanse.ExpansesID = maxID;
                MasterExpanseList.Add(NewExpanse);
                FilteredExpanseList.Add(NewExpanse);
                NewExpanse = new();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

   

    private async Task ExecuteDeleteExpanse()
    {
        if (SelectedExpanse is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete 'EXP-{SelectedExpanse.ExpansesID}'?"))
            {
                if (await _expanseService.DeleteExpanseAsync(SelectedExpanse))
                {
                    MasterExpanseList.Remove(SelectedExpanse);
                    FilteredExpanseList.Remove(SelectedExpanse);
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        CompanyDetails = CommonProperties.CompanyDetails;
        await GetExpanses();
        FilterExpanse();
         GetExpanseCategories();
    }

    private void ExecuteNewExpanseCategoryWindow()
    {
        var expanseCategoryViewModel = App.GetService<ExpanseCategoryViewModel>();
        ExpanseCategory expanseCategory = new(expanseCategoryViewModel);
        expanseCategory.ShowDialog();
    }

    private async Task ExecuteUpdateExpanse(Window win)
    {
        if (!string.IsNullOrWhiteSpace(SelectedExpanse.Amount.ToString()) && !string.IsNullOrWhiteSpace(SelectedExpanse.ExpanseCategory) && !string.IsNullOrWhiteSpace(SelectedExpanse.ExpansesDescription))
        {
            await _expanseService.UpdateExpanseAsync(SelectedExpanse);
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

   

    #endregion Command Functions

    #region Private Functions

    

    private void FilterExpanse()
    {
        Application.Current.Dispatcher.BeginInvoke
            (new Action(() => FilteredExpanseList = MasterExpanseList.Where(i => i.FilterAble.ToLower().Contains(ExpanseFilterText.ToLower())).ToObservableCollection()
        ));
    }

    private void GetExpanseCategories()
    {
        ExpanseCategoryList =  _expanseService.GetExpanseCategories();
    }

    private async Task GetExpanses()
    {
        MasterExpanseList = await _expanseService.GetExpansesAsync();
        FilteredExpanseList = MasterExpanseList;
    }

    private void MakeExpanseSummary()
    {
        if (FilteredExpanseList is not null)
        {
            ExpanseCount = FilteredExpanseList.Count();
            ExpanseTotalAmount = FilteredExpanseList.Sum(x => x.Amount);
        }
    }

    #endregion Private Functions
}