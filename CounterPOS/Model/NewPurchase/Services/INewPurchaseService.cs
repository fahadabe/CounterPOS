namespace CounterPOS.Model;

public interface INewPurchaseService
{
    Task<bool> InsertNewPurchaseTransactionAsync(NewPurchaseModel objNewPurchaseModel, ObservableCollection<NewPurchaseDetailsModel> objNewPurchaseDetailsModel);
}