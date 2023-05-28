

namespace CounterPOS.Model;

public class CustomerService : ICustomerService
{
    private readonly IDialogMessages _messages;
    public CustomerService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_GetCustomersAsync = "Select * From tblCustomers";
    private readonly string Q_GetTransactionsByCustomerAsync = "Select * From tblSaleTransactions Where CustomerID = @CustomerID ORDER by TransactionID DESC";
    //private readonly string Q_GetTransactionsDetailsByCustomerAsync = "Select * From tblSaleTransactionsDetails Where TransactionID = @TransactionID";
    //private readonly string Q_GetAddedAdditionalCharges = "Select * From tblAddedAdditionalCharges Where TransactionID = @TransactionID";
    private readonly string Q_GetLastCustomerID = "SELECT max(ID) From tblCustomers";
    private readonly string Q_UpdateCustomerAsync = "UPDATE tblCustomers SET Name = @Name, Phone = @Phone, NTN = @NTN, CNIC = @CNIC, Address = @Address, EmailAddress = @EmailAddress , Remarks = @Remarks , AddedDate = @AddedDate, BlackList = @BlackList WHERE ID = @ID";
    private readonly string Q_DeleteCustomerAsync = "DELETE FROM tblCustomers WHERE ID = @ID";
    private readonly string Q_AddCustomerAsync = "INSERT INTO tblCustomers(Name, Phone, NTN, CNIC, Address, EmailAddress, Remarks, AddedDate, BlackList) VALUES (@Name,@Phone,@NTN,@CNIC,@Address,@EmailAddress,@Remarks,@AddedDate,@BlackList)";
    public async Task<bool> AddCustomerAsync(CustomerModel objNewCustomer)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_AddCustomerAsync, new
            {
                objNewCustomer.Name,
                objNewCustomer.Phone,
                objNewCustomer.Ntn,
                objNewCustomer.Cnic,
                objNewCustomer.Address,
                objNewCustomer.EmailAddress,
                objNewCustomer.Remarks,
                AddedDate = objNewCustomer.AddedDate.ToString(SQLiteDefaultDateFormat),
                objNewCustomer.Blacklist
            });
            if (result > 0)
            {
                _messages.ShowSuccessNotification("Customer Added");
                return true;
            }
            else
            {
                _messages.ShowErrorNotification();
                return false;
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<bool> DeleteCustomerAsync(CustomerModel objDeleteCustomer)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_DeleteCustomerAsync, new
                {
                    ID = objDeleteCustomer.Id
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Customer Deleted");
                    return true;
                }
                else
                {
                    _messages.ShowErrorNotification();
                    return false;
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<bool> UpdateCustomerAsync(CustomerModel objUpdateCustomer)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            
                var result = await db.ExecuteAsync(Q_UpdateCustomerAsync, new
                {
                    objUpdateCustomer.Name,
                    objUpdateCustomer.Phone,
                    objUpdateCustomer.Ntn,
                    objUpdateCustomer.Cnic,
                    objUpdateCustomer.Address,
                    objUpdateCustomer.EmailAddress,
                    objUpdateCustomer.Remarks,
                    AddedDate = objUpdateCustomer.AddedDate.ToString(SQLiteDefaultDateFormat),
                    objUpdateCustomer.Blacklist,
                    objUpdateCustomer.Id
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Customer Updated");
                    return true;
                }
                else
                {
                    _messages.ShowErrorNotification();
                    return false;
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }



    public async Task<ObservableCollection<CustomerModel>> GetCustomersAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
                var collection = await db.QueryAsync<CustomerModel>(Q_GetCustomersAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<CustomerModel>();
        }
    }

    public async Task<ObservableCollection<TransactionModel>> GetTransactionsByCustomerAsync(int CustomerID)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

                var collection = await db.QueryAsync<TransactionModel>(Q_GetTransactionsByCustomerAsync, new { CustomerID });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<TransactionModel>();
        }
    }

    

   

    public int GetLastCustomerID()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
                 return db.ExecuteScalar<int>(Q_GetLastCustomerID);
           
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }
}