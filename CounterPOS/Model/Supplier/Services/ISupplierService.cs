namespace CounterPOS.Model;

public interface ISupplierService
{
    Task<bool> AddSupplierAsync(SupplierModel ObjNewSupplier);
    Task<bool> DeletePurchaseTransactionAsync(NewPurchaseModel objDeletePurchase);
    Task<bool> DeleteSupplierAsync(SupplierModel ObjDeleteSupplier);
    Task<ObservableCollection<SupplierModel>> GetAllSuppliersAsync();
    int GetLastSupplierID();
    Task<ObservableCollection<NewPurchaseModel>> GetPurchaseBySupplierAsync(int SupplierID);
    Task<ObservableCollection<NewPurchaseDetailsModel>> GetTransactionsDetailsBySupplierAsync(int PurchaseID);
    Task<bool> UpdateSupplierAsync(SupplierModel ObjUpdateSupplier);
}