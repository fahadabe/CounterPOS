namespace CounterPOS.Model;

public class LoginService : ILoginService
{
    private readonly IDialogMessages _messages;
    private readonly IImageHelper _imageHelper;
    public LoginService(IDialogMessages messages, IImageHelper imageHelper)
    {
        _messages = messages;
        _imageHelper = imageHelper;
    }

    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_Login = "SELECT * from tblUser WHERE Username = @username AND Password = @password";
    private readonly string Q_InsertInitialData1 = "INSERT INTO tblCompany (Name, Country, StartYear, Address1, Address2, Email, Phone, Logo, PrintMessage1, PrintMessage2, CurrencySymbol) VALUES (@a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k)";
    private readonly string Q_InsertInitialData2 = "INSERT INTO tblPaymentType (PaymentType) VALUES (@PaymentType)";
    private readonly string Q_InsertInitialData3 = "INSERT INTO tblUser(Username, Password, AddAccount, EditAccount, DeleteAccount, AddInventory, EditInventory, DeleteInventory, CreateInvoice, DeleteInvoice, AddExpanse, EditExpanse, DeleteExpanse, InteractDashboard, ViewReports, EditSettings) VALUES(@a, @b, @c, @d, @e, @f, @g, @h, @i, @j, @k, @l, @m, @n, @o, @p)";
    private readonly string Q_InsertInitialData4 = "INSERT INTO tblFBRParameters (POSID) VALUES (@POSID)";
    public UserModel Login(string username, string password)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var user = db.QueryFirstOrDefault<UserModel>(Q_Login, new
            {
                username,
                password
            });
            return user;
        }
        catch (System.Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return null;
        }
    }

    public bool InsertInitialData(CompanyModel companyDetails, UserModel newUser, FBRParametersModel newFBRParameters, string picture)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
            using IDbTransaction transaction = db.BeginTransaction();
            
                try
                {

                    db.Execute(Q_InsertInitialData1, new
                    {
                        a = companyDetails.Name,
                        b = companyDetails.Country,
                        c = companyDetails.StartYear.ToString(SQLiteDefaultDateFormat),
                        d = companyDetails.Address1,
                        e = companyDetails.Address2,
                        f = companyDetails.Email,
                        g = companyDetails.Phone,
                        h = _imageHelper.NewBitmapToByte(_imageHelper.NewImageToBitmap(picture)),
                        i = companyDetails.PrintMessage1,
                        j = companyDetails.PrintMessage2,
                        k = companyDetails.CurrencySymbol
                    });

                    List<string> paymentTypes = new()
                        {
                             "Cash",
                             "Bank",
                             "Online"
                        };

                    foreach (var item in paymentTypes)
                    {
                        db.Execute(Q_InsertInitialData2, new
                        {
                            PaymentType = item
                        });
                    }

                    db.Execute(Q_InsertInitialData3, new
                    {
                        a = newUser.Username,
                        b = newUser.Password,
                        c = 1,
                        d = 1,
                        e = 1,
                        f = 1,
                        g = 1,
                        h = 1,
                        i = 1,
                        j = 1,
                        k = 1,
                        l = 1,
                        m = 1,
                        n = 1,
                        o = 1,
                        p = 1
                    });

                    db.Execute(Q_InsertInitialData4, new
                    {
                        newFBRParameters.POSID
                    });

                    transaction.Commit();
                    return transaction.ReturnSuccess();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    _messages.ShowErrorMessage(e.Message);
                    return false;
                }
                finally
                {
                    db.Close();
                    transaction.Dispose();
                }
            
        }
        catch (Exception e)
        {

            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }
}