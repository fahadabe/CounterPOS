namespace CounterPOS.ViewModel;

public partial class FBRParametersViewModel : ObservableObject
{
    private readonly IFBRParametersService _fBRParametersService;
    private readonly IDialogMessages _messages;
    public FBRParametersViewModel(IFBRParametersService fBRParametersService, IDialogMessages messages)
    {
        _fBRParametersService = fBRParametersService;
        _messages = messages;
    }

    #region Commands

    public DelegateCommand CheckAPIConnectionCommand => new DelegateCommand(ExecuteCheckAPIConnection);
    public DelegateCommand CheckServiceStatusCommand => new DelegateCommand(ExecuteCheckServiceStatus);
    public DelegateCommand LoadDataCommand => new DelegateCommand(ExecuteLoadData);
    public AsyncCommand SaveFBRParametersCommand => new AsyncCommand(ExecuteSaveFBRParameters, CanExecuteSaveFBRParameters);

    #endregion Commands

    #region Properties
    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private FBRParametersModel _FBRParametersModel = new();
    [ObservableProperty]
    private string _ServiceStatus = "";

    

    

    #endregion Properties

    #region Command Functions

    private bool CanExecuteSaveFBRParameters()
    {
        return CurrentUser?.EditSettings ?? false;
    }

    private void ExecuteCheckAPIConnection()
    {
        try
        {
            using (HttpClient client = new())
            {
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    private void ExecuteCheckServiceStatus()
    {
        CheckServiceStatus();
    }

    private void ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;
        FBRParametersModel = _fBRParametersService.GetFBRParametersAsync();

        CheckServiceStatus();
    }

    private async Task ExecuteSaveFBRParameters()
    {
        try
        {
            await _fBRParametersService.UpdateFBRParametersAsync(FBRParametersModel);
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    #endregion Command Functions

    #region Private Functions

    private void CheckServiceStatus()
    {
        try
        {
            ServiceController sc = new ServiceController("IMS_Fiscalization");
            switch (sc.Status)
            {
                case ServiceControllerStatus.Running:
                    ServiceStatus = "Running";
                    break;

                case ServiceControllerStatus.Stopped:
                    ServiceStatus = "Stopped";
                    break;

                case ServiceControllerStatus.Paused:
                    ServiceStatus = "Paused";
                    break;

                case ServiceControllerStatus.StopPending:
                    ServiceStatus = "Stopping";
                    break;

                case ServiceControllerStatus.StartPending:
                    ServiceStatus = "Starting";
                    break;

                default:
                    ServiceStatus = "Getting status...";
                    break;
            }
        }
        catch (Exception e)
        {
            _messages.ShowErrorMessage(e.Message);
        }
    }

    #endregion Private Functions
}