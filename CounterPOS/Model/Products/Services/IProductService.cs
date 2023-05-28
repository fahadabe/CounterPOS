namespace CounterPOS.Model;

public interface IProductService
{
    Task<bool> AddProductAsync(ProductModel ObjNewProduct, string picture);
    Task<bool> AddSizedProductAsync(ProductModel ObjNewProduct, string picture, ObservableCollection<ProductSizes> ObjProductSize);
    Task<bool> CheckIfProductExistAsync(string productName);
    Task<bool> DeleteProductAsync(ProductModel objDeleteProduct);
    Task<ObservableCollection<ProductSizes>> GeneratingNewProductSizesAsync(int productID, IDbConnection db, ObservableCollection<ProductSizes> productSizesFromUser);
    Task<ObservableCollection<ProductModel>> GetAllProductsAsync();
    int GetLastProductID();
    Task<int> GetLastRowIDAsync(IDbConnection db);
    Task<ObservableCollection<ProductPerformanceModel>> GetProductPerformance(string productName);
    ObservableCollection<ProductSizes> GetSizesForSelectedProduct(int productId);
    Task<TransactionModel> GetTransactionAsync(int transactionID);
    Task<bool> UpdateProductAsync(ProductModel objUpdateProduct, object picture, bool isImageChanged, object selectedPicture);
    Task<bool> UpdateSizedProductAsync(ProductModel objUpdateProduct, ObservableCollection<ProductSizes> ObjProductSize, object picture, bool isImageChanged, object selectedPicture);
}