namespace CounterPOS.Model;

public class ExpanseService : IExpanseService
{
    private readonly IDialogMessages _messages;
    public ExpanseService(IDialogMessages messages)
    {
        _messages = messages;
    }

    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_GetExpanseCategoriesAsync = "Select * From tblExpanseCategory";
    private readonly string Q_GetExpansesAsync = "Select * From tblExpanses";
    private readonly string Q_AddExpanseCategoryAsync = "INSERT INTO tblExpanseCategory (Category) VALUES (@Category)";
    private readonly string Q_AddExpanseAsync = "INSERT INTO tblExpanses (ExpansesDescription, Amount, ExpanseDate, ExpanseCategory, Note) VALUES (@ExpansesDescription,@Amount,@ExpanseDate,@ExpanseCategory,@Note)";
    private readonly string Q_UpdateExpanseAsync = "Update tblExpanses Set ExpansesDescription = @ExpansesDescription, Amount = @Amount, ExpanseDate = @ExpanseDate, ExpanseCategory = @ExpanseCategory, Note = @Note Where ExpansesID = @ExpansesID";
    private readonly string Q_DeleteExpanseAsync = "DELETE FROM tblExpanses WHERE ExpansesID = @ExpansesID";
    private readonly string Q_GetLastExpanseID = "SELECT max(ExpansesID) from tblExpanses";
    public ObservableCollection<ExpanseCategoryModel> GetExpanseCategories()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = db.Query<ExpanseCategoryModel>(Q_GetExpanseCategoriesAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ExpanseCategoryModel>();
        }
    }

    public async Task<ObservableCollection<ExpanseModel>> GetExpansesAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<ExpanseModel>(Q_GetExpansesAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<ExpanseModel>();
        }
    }

    public async Task<bool> AddExpanseCategoryAsync(ExpanseCategoryModel ObjNewCategory)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_AddExpanseCategoryAsync, new
                {
                    ObjNewCategory.Category,
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

    public async Task<bool> AddExpanseAsync(ExpanseModel ObjNewExpanse)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_AddExpanseAsync, new
                {
                    ObjNewExpanse.ExpansesDescription,
                    ObjNewExpanse.Amount,
                    ExpanseDate = ObjNewExpanse.ExpanseDate.ToString(SQLiteDefaultDateFormat),
                    ObjNewExpanse.ExpanseCategory,
                    ObjNewExpanse.Note
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Expanse Added");
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

    public async Task<bool> UpdateExpanseAsync(ExpanseModel ObjUpdateExpanse)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_UpdateExpanseAsync, new
                {
                    ObjUpdateExpanse.ExpansesDescription,
                    ObjUpdateExpanse.Amount,
                    ExpanseDate = ObjUpdateExpanse.ExpanseDate.ToString(SQLiteDefaultDateFormat),
                    ObjUpdateExpanse.ExpanseCategory,
                    ObjUpdateExpanse.Note,
                    ObjUpdateExpanse.ExpansesID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Expanse Updated");
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

    public async Task<bool> DeleteExpanseAsync(ExpanseModel ObjDeleteExoanse)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_DeleteExpanseAsync, new
                {
                    ObjDeleteExoanse.ExpansesID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Expanse Deleted");
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

    public int GetLastExpanseID()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return db.ExecuteScalar<int>(Q_GetLastExpanseID);
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }

    }

}