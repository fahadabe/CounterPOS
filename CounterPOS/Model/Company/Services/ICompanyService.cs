namespace CounterPOS.Model;

public interface ICompanyService
{
    CompanyModel? GetCompanyInfo();
    Task<bool> UpdateCompanyInfoAsync(CompanyModel ObjUpdateCompany, string? picture, bool isImageChanged, object? selectedPicture);
}