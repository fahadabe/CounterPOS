namespace CounterPOS.Model;

public interface ICustomerService
{
    Task<bool> AddCustomerAsync(CustomerModel objNewCustomer);
    Task<bool> DeleteCustomerAsync(CustomerModel objDeleteCustomer);
    Task<ObservableCollection<CustomerModel>> GetCustomersAsync();
    int GetLastCustomerID();
    Task<ObservableCollection<TransactionModel>> GetTransactionsByCustomerAsync(int CustomerID);
    Task<bool> UpdateCustomerAsync(CustomerModel objUpdateCustomer);
}