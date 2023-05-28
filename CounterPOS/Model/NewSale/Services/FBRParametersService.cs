namespace CounterPOS.Model;

public class FBRParametersService : IFBRParametersService
{
    private readonly IDialogMessages _messages;
    public FBRParametersService(IDialogMessages messages)
    {
        _messages = messages;
    }
    private readonly string Q_GetFBRParametersAsync = "SELECT * FROM tblFBRParameters";
    private readonly string Q_UpdateFBRParametersAsync = "UPDATE tblFBRParameters SET POSID = @POSID";
    public FBRParametersModel? GetFBRParametersAsync()
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            return db.QuerySingle<FBRParametersModel>(Q_GetFBRParametersAsync);
            
        }
        catch (Exception)
        {
            _messages.ShowWarningNotification("FBR Parameters are not set.");
            return null;
        }
    }

    public async Task<bool> UpdateFBRParametersAsync(FBRParametersModel objUpdateFBRParameters)
    {
        try
        {
            using IDbConnection db = new SQLiteConnection(ConnectionHelper.CnnVal());
            var result = await db.ExecuteAsync(Q_UpdateFBRParametersAsync, new
                {
                    objUpdateFBRParameters.POSID
                });
                if (result > 0)
                {
                    _messages.ShowSuccessNotification("Parameters Updated");
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
}