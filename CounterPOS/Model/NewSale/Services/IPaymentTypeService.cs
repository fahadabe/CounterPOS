namespace CounterPOS.Model;

public interface IPaymentTypeService
{
    Task AddPaymentTypeAsync(PaymentTypeModel objNewPaymentType);
    bool CheckIfPaymentTypeExist(string PaymentType);
    Task<ObservableCollection<PaymentTypeModel>> GetPaymentTypeListAsync();
}