namespace CounterPOS.Model;

public class UserManagementService : IUserManagementService
{
    private readonly IDialogMessages _messages;
    public UserManagementService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string Q_GetUserListAsync = "SELECT * FROM tblUser";
    private readonly string Q_CheckIfUserExist = "SELECT count(*) FROM tblUser WHERE Username = @username";
    private readonly string Q_AddUserAsync = "INSERT INTO tblUser(Username, Password) VALUES (@a,@b)";
    private readonly string Q_UpdateUserAsync = "UPDATE tblUser SET Username = @a, Password = @b, AddAccount = @c, EditAccount = @d, DeleteAccount = @e, AddInventory = @f, EditInventory = @g, DeleteInventory = @h, CreateInvoice = @i, DeleteInvoice = @j, AddExpanse = @k, EditExpanse = @l, DeleteExpanse = @m, InteractDashboard = @n, ViewReports = @o, EditSettings = @p WHERE UserID = @id";
    private readonly string Q_DeleteUserAsync = "DELETE FROM tblUser WHERE UserID = @a";
    public async Task<ObservableCollection<UserModel>> GetUserListAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<UserModel>(Q_GetUserListAsync);
            return collection.ToObservableCollection();
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<UserModel>();
        }
    }

    public bool CheckIfUserExist(string username)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = db.ExecuteScalar<bool>(Q_CheckIfUserExist, new
            {
                username,
            });
            return result;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task AddUserAsync(UserModel ObjNewUser)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.ExecuteAsync(Q_AddUserAsync, new
                {
                    a = ObjNewUser.Username,
                    b = ObjNewUser.Password,
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("User Added");
                }
                else
                {
                    _messages.ShowErrorNotification();
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    public async Task<bool> UpdateUserAsync(UserModel ObjUpdateUser)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_UpdateUserAsync, new
                {
                    a = ObjUpdateUser.Username,
                    b = ObjUpdateUser.Password,
                    c = ObjUpdateUser.AddAccount ? 1 : 0,
                    d = ObjUpdateUser.EditAccount ? 1 : 0,
                    e = ObjUpdateUser.DeleteAccount ? 1 : 0,
                    f = ObjUpdateUser.AddInventory ? 1 : 0,
                    g = ObjUpdateUser.EditInventory ? 1 : 0,
                    h = ObjUpdateUser.DeleteInventory ? 1 : 0,
                    i = ObjUpdateUser.CreateInvoice ? 1 : 0,
                    j = ObjUpdateUser.DeleteInvoice ? 1 : 0,
                    k = ObjUpdateUser.AddExpanse ? 1 : 0,
                    l = ObjUpdateUser.EditExpanse ? 1 : 0,
                    m = ObjUpdateUser.DeleteExpanse ? 1 : 0,
                    n = ObjUpdateUser.InteractDashboard ? 1 : 0,
                    o = ObjUpdateUser.ViewReports ? 1 : 0,
                    p = ObjUpdateUser.EditSettings ? 1 : 0,
                    id = ObjUpdateUser.UserID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("User Updated");
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

    public async Task DeleteUserAsync(UserModel ObjDeleteUser)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            
                if (!Equals(ObjDeleteUser.Username, CommonProperties.CurrentUser))
                {
                    var result = await db.ExecuteAsync(Q_DeleteUserAsync, new
                    {
                        a = ObjDeleteUser.UserID
                    });
                    if (result > 0)
                    {
                        _messages.ShowSuccessNotification("User Deleted");
                    }
                    else
                    {
                        _messages.ShowErrorNotification();
                    }
                }
                else
                {
                    _messages.ShowInfoNotification("Cannot delete yourself");
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }
}