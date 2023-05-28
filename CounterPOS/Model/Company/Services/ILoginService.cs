namespace CounterPOS.Model;

public interface ILoginService
{
    bool InsertInitialData(CompanyModel companyDetails, UserModel newUser, FBRParametersModel newFBRParameters, string picture);
    UserModel Login(string username, string password);
}