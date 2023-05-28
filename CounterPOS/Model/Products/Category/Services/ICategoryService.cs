namespace CounterPOS.Model;

public interface ICategoryService
{
    Task<bool> AddCategoryAsync(CategoryModel objNewCategory);
    Task<bool> DeleteCategoryAsync(CategoryModel objDeleteCategory);
    Task<ObservableCollection<CategoryModel>> GetAllCategoriesAsync();
    Task<bool> UpdateCategoryAsync(CategoryModel objUpdateCategory);
}