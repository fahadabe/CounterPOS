namespace CounterPOS.Model;

public class AdditionalChargesService : IAdditionalChargesService
{
    private readonly IDialogMessages _messages;
    public AdditionalChargesService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string Q_UpdateAdditionalCharges1 = "Delete From tblAdditionalCharges";
    private readonly string Q_UpdateAdditionalCharges2 = "INSERT INTO tblAdditionalCharges (ChargeType, ChargeAmount) VALUES (@ChargeType, @ChargeAmount)";
    private readonly string Q_GetAdditionalChargesAsync = "Select * From tblAdditionalCharges";
    public async Task<bool> UpdateAdditionalCharges(ObservableCollection<AdditionalChargesModel> objAdditionalCharges)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            db.Open();
                using (var transaction = db.BeginTransaction())
                {

                    db.Execute(Q_UpdateAdditionalCharges1);


                    foreach (var item in objAdditionalCharges)
                    {
                        await db.ExecuteAsync(Q_UpdateAdditionalCharges2, new
                        {
                            item.ChargeType,
                            item.ChargeAmount,
                        });
                    }
                    transaction.Commit();
                    if (transaction.ReturnSuccess())
                    {
                        _messages.ShowSuccessNotification("Charges Updated");
                        return true;
                    }
                    else
                    {
                        _messages.ShowErrorNotification();
                        return false;
                    }
                }
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return false;
        }
    }

    public ObservableCollection<AdditionalChargesModel> GetAdditionalChargesAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var collection = db.Query<AdditionalChargesModel>(Q_GetAdditionalChargesAsync);
                return collection.ToObservableCollection();
            
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
            return TOC.Empty<AdditionalChargesModel>();
        }
    }
}