using System.Collections.ObjectModel;

namespace CounterPOS.Model;

public class SupplierService : ISupplierService
{
    private readonly IDialogMessages _messages;
    public SupplierService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string SQLiteDefaultDateFormat = "yyyy-MM-dd";
    private readonly string Q_GetAllSuppliersAsync = "Select * From tblSuppliers";
    private readonly string Q_GetPurchaseBySupplierAsync = "Select * From tblPurchaseTransactions Where SupplierID = @SupplierID ORDER by SupplierID DESC";
    private readonly string Q_DeletePurchaseTransactionAsync1 = "DELETE FROM tblPurchaseTransactions WHERE PurchaseID = @a";
    private readonly string Q_DeletePurchaseTransactionAsync2 = "DELETE FROM tblPurchaseTransactionsDetails WHERE PurchaseID = @b";
    private readonly string Q_GetTransactionsDetailsBySupplierAsync = "Select * From tblPurchaseTransactionsDetails Where PurchaseID = @PurchaseID";
    private readonly string Q_AddSupplierAsync = "INSERT INTO tblSuppliers(SupplierName, Email, Phone, Location, Website, Note, AddedDate ) VALUES(@b, @c, @d, @e, @f, @g, @h)";
    private readonly string Q_UpdateSupplierAsync = "UPDATE tblSuppliers SET SupplierName = @b, Email = @c, Phone = @d, Location = @e, Website = @f, Note = @g, AddedDate = @h WHERE SupplierID = @i";
    private readonly string Q_DeleteSupplierAsync = "DELETE FROM tblSuppliers WHERE SupplierID = @a";
    private readonly string Q_GetLastSupplierID = "SELECT max(SupplierID) from tblSuppliers";
    public async Task<ObservableCollection<SupplierModel>> GetAllSuppliersAsync()
    {

        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<SupplierModel>(Q_GetAllSuppliersAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<SupplierModel>();
        }
    }

    public async Task<ObservableCollection<NewPurchaseModel>> GetPurchaseBySupplierAsync(int SupplierID)
    {

        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<NewPurchaseModel>(Q_GetPurchaseBySupplierAsync, new
                {
                    SupplierID
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<NewPurchaseModel>();
        }
    }

    public async Task<bool> DeletePurchaseTransactionAsync(NewPurchaseModel objDeletePurchase)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
                using IDbTransaction transaction = db.BeginTransaction();
                    try
                    {


                        await db.ExecuteAsync(Q_DeletePurchaseTransactionAsync1, new
                        {
                            a = objDeletePurchase.PurchaseID
                        });

                        await db.ExecuteAsync(Q_DeletePurchaseTransactionAsync2, new
                        {
                            b = objDeletePurchase.PurchaseID
                        });
                        transaction.Commit();
                        db.Close();
                        if (transaction.ReturnSuccess())
                        {
                            _messages.ShowSuccessNotification("Transaction Deleted");
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
                        transaction.Rollback();
                        _messages.ShowErrorMessage(e.Message);
                        return false;
                    }
                
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task<ObservableCollection<NewPurchaseDetailsModel>> GetTransactionsDetailsBySupplierAsync(int PurchaseID)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<NewPurchaseDetailsModel>(Q_GetTransactionsDetailsBySupplierAsync, new
                {
                    PurchaseID
                });
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<NewPurchaseDetailsModel>();
        }
    }

    public async Task<bool> AddSupplierAsync(SupplierModel ObjNewSupplier)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_AddSupplierAsync, new
                {
                    b = ObjNewSupplier.SupplierName,
                    c = ObjNewSupplier.Email,
                    d = ObjNewSupplier.Phone,
                    e = ObjNewSupplier.Location,
                    f = ObjNewSupplier.Website,
                    g = ObjNewSupplier.Note,
                    h = ObjNewSupplier.AddedDate.ToString(SQLiteDefaultDateFormat)
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Supplier Added");
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

    public async Task<bool> UpdateSupplierAsync(SupplierModel ObjUpdateSupplier)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.ExecuteAsync(Q_UpdateSupplierAsync, new
                {
                    b = ObjUpdateSupplier.SupplierName,
                    c = ObjUpdateSupplier.Email,
                    d = ObjUpdateSupplier.Phone,
                    e = ObjUpdateSupplier.Location,
                    f = ObjUpdateSupplier.Website,
                    g = ObjUpdateSupplier.Note,
                    h = ObjUpdateSupplier.AddedDate.ToString(SQLiteDefaultDateFormat),
                    i = ObjUpdateSupplier.SupplierID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Supplier Updated");
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

    public async Task<bool> DeleteSupplierAsync(SupplierModel ObjDeleteSupplier)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_DeleteSupplierAsync, new
                {
                    a = ObjDeleteSupplier.SupplierID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Supplier Deleted");
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

    public int GetLastSupplierID()
    {
        int lastCustomerID = 0;
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return lastCustomerID = db.ExecuteScalar<int>(Q_GetLastSupplierID);
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return default;
        }
    }
}