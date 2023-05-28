namespace CounterPOS.Model;

public class CompanyService : ICompanyService
{
    private readonly IDialogMessages _messages;
    private readonly IImageHelper _imageHelper;
    public CompanyService(IDialogMessages messages, IImageHelper imageHelper)
    {
        _messages = messages;
        _imageHelper = imageHelper;
    }
    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_GetCompanyInfo = "SELECT * FROM tblCompany";
    private readonly string Q_UpdateCompanyInfoAsync = "UPDATE tblCompany SET Name = @Name, Country = @Country , StartYear = @StartYear, Address1 = @Address1, Address2 = @Address2 , Email = @Email , Phone = @Phone, Logo = @Logo, PrintMessage1 = @PrintMessage1, PrintMessage2 = @PrintMessage2, CurrencySymbol = @CurrencySymbol";

    public CompanyModel? GetCompanyInfo()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = db.QuerySingle<CompanyModel>(Q_GetCompanyInfo);
            return result;
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return null;
        }
    }

    public async Task<bool> UpdateCompanyInfoAsync(CompanyModel ObjUpdateCompany, string? picture, bool isImageChanged, object? selectedPicture)
    {
        try
        {
            using var db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.ExecuteAsync(Q_UpdateCompanyInfoAsync, new
            {
                ObjUpdateCompany.Name,
                ObjUpdateCompany.Country,
                StartYear = ObjUpdateCompany.StartYear.ToString(SQLiteDefaultDateFormat),
                ObjUpdateCompany.Address1,
                ObjUpdateCompany.Address2,
                ObjUpdateCompany.Email,
                ObjUpdateCompany.Phone,
                Logo = _imageHelper.EditImageToBitmapToByte(picture, isImageChanged, selectedPicture),
                ObjUpdateCompany.PrintMessage1,
                ObjUpdateCompany.PrintMessage2,
                ObjUpdateCompany.CurrencySymbol
            });

            if (result > 0)
            {
                _messages.ShowSuccessNotification("Company Updated");
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