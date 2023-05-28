namespace CounterPOS.Model;

public interface IExpanseService
{
    Task<bool> AddExpanseAsync(ExpanseModel ObjNewExpanse);
    Task<bool> AddExpanseCategoryAsync(ExpanseCategoryModel ObjNewCategory);
    Task<bool> DeleteExpanseAsync(ExpanseModel ObjDeleteExoanse);
    ObservableCollection<ExpanseCategoryModel> GetExpanseCategories();
    Task<ObservableCollection<ExpanseModel>> GetExpansesAsync();
    int GetLastExpanseID();
    Task<bool> UpdateExpanseAsync(ExpanseModel ObjUpdateExpanse);
}