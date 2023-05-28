namespace CounterPOS.Model;

public class PaymentTypeService : IPaymentTypeService
{
    private readonly IDialogMessages _messages;
    public PaymentTypeService(IDialogMessages messages)
    {
        _messages = messages;
    }

    private readonly string Q_GetPaymentTypeListAsync = "Select * From tblPaymentType";
    private readonly string Q_CheckIfPaymentTypeExist = "SELECT count(*) FROM tblPaymentType WHERE PaymentType = @PaymentType";
    private readonly string Q_AddPaymentTypeAsync = "INSERT INTO tblPaymentType (PaymentType) VALUES (@PaymentType)";
    public async Task<ObservableCollection<PaymentTypeModel>> GetPaymentTypeListAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = await db.QueryAsync<PaymentTypeModel>(Q_GetPaymentTypeListAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<PaymentTypeModel>();
        }
    }

    public bool CheckIfPaymentTypeExist(string PaymentType)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            return db.ExecuteScalar<bool>(Q_CheckIfPaymentTypeExist, new { PaymentType });
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public async Task AddPaymentTypeAsync(PaymentTypeModel objNewPaymentType)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());

            var result = await db.ExecuteAsync(Q_AddPaymentTypeAsync, new
                {
                    objNewPaymentType.PaymentType,
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Payment Type Added");
                }
                else
                {
                    _messages.ShowErrorNotification();
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }
}