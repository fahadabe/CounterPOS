namespace CounterPOS.Model;

public class CategoryService : ICategoryService
{
    private readonly IDialogMessages _messages;
    public CategoryService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string Q_GetAllCategoriesAsync = "Select * From tblCategory";
    private readonly string Q_AddCategoryAsync = "INSERT INTO tblCategory (Category) VALUES (@a)";
    private readonly string Q_UpdateCategoryAsync = "UPDATE tblCategory SET Category = @a WHERE CategoryID = @b";
    private readonly string Q_DeleteCategoryAsync = "DELETE FROM tblCategory WHERE CategoryID = @a";
    public async Task<ObservableCollection<CategoryModel>> GetAllCategoriesAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<CategoryModel>(Q_GetAllCategoriesAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<CategoryModel>();
        }
    }

    public async Task<bool> AddCategoryAsync(CategoryModel objNewCategory)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_AddCategoryAsync, new
                {
                    a = objNewCategory.Category,
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Category Added");
                    return true;
                }
                else
                {
                    _messages.ShowErrorNotification();
                    return false;
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateCategoryAsync(CategoryModel objUpdateCategory)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.ExecuteAsync(Q_UpdateCategoryAsync, new
                {
                    a = objUpdateCategory.Category,
                    b = objUpdateCategory.CategoryID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Category Updated");
                    return true;
                }
                else
                {
                    _messages.ShowErrorNotification();
                    return false;
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(CategoryModel objDeleteCategory)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_DeleteCategoryAsync, new
                {
                    a = objDeleteCategory.CategoryID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Category Deleted");
                    return true;
                }
                else
                {
                    _messages.ShowErrorNotification();
                    return false;
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }
}