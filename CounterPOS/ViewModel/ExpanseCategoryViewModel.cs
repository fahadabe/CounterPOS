namespace CounterPOS.ViewModel;

public partial class ExpanseCategoryViewModel : ObservableObject
{
    private readonly IExpanseService _expanseService;
    private readonly IDialogMessages _messages;
    public ExpanseCategoryViewModel(IExpanseService expanseService, IDialogMessages messages)
    {
        _expanseService = expanseService;
        _messages = messages;
        LoadData();
    }
    public AsyncCommand AddExpanseCategoryCommand => new AsyncCommand(ExecuteAddExpanseCategory, CanExecuteAddExpanseCategory);

    [ObservableProperty]
    private ObservableCollection<ExpanseCategoryModel> _ExpanseCategoryList = new();


    [ObservableProperty]
    private UserModel _CurrentUser = new();


    [ObservableProperty]
    private ExpanseCategoryModel _NewExpanseCategory = new();

    
    private void LoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        ExpanseCategoryList =  _expanseService.GetExpanseCategories();
    }

    private bool CanExecuteAddExpanseCategory()
    {
        return CurrentUser?.AddAccount ?? false;
    }

    private async Task ExecuteAddExpanseCategory()
    {
        if (!string.IsNullOrWhiteSpace(NewExpanseCategory.Category))
        {
            if (await _expanseService.AddExpanseCategoryAsync(NewExpanseCategory))
            {
                LoadData();
                NewExpanseCategory = new();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }
}