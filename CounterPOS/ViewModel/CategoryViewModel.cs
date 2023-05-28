namespace CounterPOS.ViewModel;

public partial class CategoryViewModel : ObservableObject
{
    private readonly ICategoryService _categoryService;
    private readonly IDialogMessages _messages;
    public CategoryViewModel(ICategoryService categoryService, IDialogMessages messages)
    {

        _categoryService = categoryService;
        _messages = messages;
    }
    #region Commands

    public AsyncCommand AddCategoryCommand => new AsyncCommand(ExecuteAddCategory, CanExecuteAddCategory);
    public AsyncCommand DeleteCategoryCommand => new AsyncCommand(ExecuteDeleteCategory, CanExecuteDeleteCategory);
    public AsyncCommand EditCategoryCommand => new AsyncCommand(ExecuteEditCategory, CanExecuteEditCategory);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);

    #endregion Commands

    #region Properties

    [ObservableProperty]
    private ObservableCollection<CategoryModel>? _CategoryList = new();
    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private CategoryModel _NewCategory = new();
    [ObservableProperty]
    private CategoryModel _SelectedCategory = new();

   

    #endregion Properties

    #region Command Functions

    private bool CanExecuteAddCategory()
    {
        return CurrentUser.AddInventory && NewCategory is not null;
    }

    private bool CanExecuteDeleteCategory()
    {
        return CurrentUser.DeleteInventory && SelectedCategory is not null;
    }

    private bool CanExecuteEditCategory()
    {
        return CurrentUser.EditInventory && SelectedCategory is not null;
    }


    
    private async Task ExecuteAddCategory()
    {
        if (!string.IsNullOrWhiteSpace(NewCategory.Category))
        {
            if (await _categoryService.AddCategoryAsync(NewCategory))
            {
                await ExecuteLoadData();
                NewCategory = new();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private async Task ExecuteDeleteCategory()
    {
        if (SelectedCategory is not null)
        {
            if (await _messages.AskUser($"Are you sure to delete '{SelectedCategory.Category}' Category?"))
            {
                if (await _categoryService.DeleteCategoryAsync(SelectedCategory))
                {
                    await ExecuteLoadData();
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private async Task ExecuteEditCategory()
    {
        if (!string.IsNullOrWhiteSpace(SelectedCategory?.Category))
        {
            if (await _categoryService.UpdateCategoryAsync(SelectedCategory))
            {
                await ExecuteLoadData();
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.FieldsEmptyErrorMessage);
        }
    }

    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        CategoryList = await _categoryService.GetAllCategoriesAsync();
    }

    #endregion Command Functions
}