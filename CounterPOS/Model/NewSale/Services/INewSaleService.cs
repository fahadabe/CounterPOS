namespace CounterPOS.Model;

public interface INewSaleService
{
    Task CreateProductMenuAsync(ObservableCollection<Tabs> tabCollection);
    Task<bool> DeleteAnSaveTransactionAsync(TransactionModel ObjTransactionModel);
    Task<bool> DeleteTransactionPermanently(ObservableCollection<TransactionModel> transactions);
    Task<ObservableCollection<BaseProductModel>> GetAllProducts();
    Task<ObservableCollection<BaseProductModel>> GetAllProducts(string category, IDbConnection db);
    Task<ObservableCollection<TransactionModel>> GetDeletedTransactions();
    int GetLasRow();
    int GetLatestRowFromDatabase(IDbConnection db);
    Task<bool> InsertTransactionAsync(TransactionModel ObjTransactionModel, ObservableCollection<CartModel> ObjTransactionDetails, FBRResponseModel fBRResponseModel);
    int ReturnLatestRow();
}