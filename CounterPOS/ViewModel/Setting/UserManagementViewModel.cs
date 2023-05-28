namespace CounterPOS.ViewModel;

public partial class UserManagementViewModel : ObservableObject
{
    private readonly IUserManagementService _userManagementService;
    private readonly IDialogMessages _messages;
    public UserManagementViewModel(IUserManagementService userManagementService, IDialogMessages messages)
    {

        _userManagementService = userManagementService;
        _messages = messages;
    }

    #region Commands

    public AsyncCommand AddUserCommand => new AsyncCommand(ExecuteAddUser, CanExecuteAdduser);

    public AsyncCommand DeleteUserCommand => new AsyncCommand(ExecuteDeleteUser, CanExecuteDeleteuser);
    public AsyncCommand LoadDataCommand => new AsyncCommand(ExecuteLoadData);
    public DelegateCommand OpenUpdateUserWindowCommand => new DelegateCommand(ExecuteOpenUpdateUserWindow);
    public AsyncCommand RefreshUserCommand => new AsyncCommand(ExecuteRefreshUser);
    public AsyncCommand UpdateUserCommand => new AsyncCommand(ExecuteUpdateUser, CanExecuteUpdateUser);
    #endregion

    #region Properties

    [ObservableProperty]
    private UserModel _CurrentUser = new();
    [ObservableProperty]
    private UserModel _NewUser = new();
    [ObservableProperty]
    private UserModel _SelectedUser = new();
    [ObservableProperty]
    private ObservableCollection<UserModel> _UserList = new();

    


    
    #endregion
    #region Command Functions

   
    private bool CanExecuteAdduser()
    {
        return CurrentUser.AddAccount;
    }

    private bool CanExecuteDeleteuser()
    {
        return CurrentUser.DeleteAccount;
    }

    private bool CanExecuteUpdateUser()
    {
        return CurrentUser.EditAccount;
    }

    private async Task ExecuteAddUser()
    {
        if (!string.IsNullOrEmpty(NewUser.Username) && !string.IsNullOrEmpty(NewUser.Password))
        {
            if (_userManagementService.CheckIfUserExist(NewUser.Username))
            {
                _messages.ShowWarningNotification($"{NewUser.Username} already exist.");
            }
            else
            {
                await _userManagementService.AddUserAsync(NewUser);
                UserList = await _userManagementService.GetUserListAsync();
            }
        }
        else
        {
            _messages.ShowWarningNotification("Username & Password cannot be empty");
        }
    }

    private async Task ExecuteDeleteUser()
    {
        if (SelectedUser is not null)
        {
            if (string.Equals(SelectedUser.Username, CurrentUser.Username))
            {
                _messages.ShowInfoNotification("Cannot delete yourself.");
            }
            else
            {
                if (await _messages.AskUser($"Are you sure to delete '{SelectedUser.Username}'?"))
                {
                    await _userManagementService.DeleteUserAsync(SelectedUser);
                    UserList = await _userManagementService.GetUserListAsync();
                }
            }
        }
        else
        {
            _messages.ShowWarningNotification(Constants.SelectAtLeatOneRecord);
        }
    }

    private async Task ExecuteLoadData()
    {
        CurrentUser = CommonProperties.CurrentUser;

        UserList = await _userManagementService.GetUserListAsync();
    }

    private void ExecuteOpenUpdateUserWindow()
    {
        UpdateUserWindow updateuser = new()
        {
            DataContext = this
        };
        updateuser.ShowDialog();
    }

    private async Task ExecuteRefreshUser()
    {
        UserList = await _userManagementService.GetUserListAsync();
    }

    private async Task ExecuteUpdateUser()
    {
        if (SelectedUser is not null && !string.IsNullOrEmpty(SelectedUser.Username) && !string.IsNullOrEmpty(SelectedUser.Password))
        {
            
            if(await _userManagementService.UpdateUserAsync(SelectedUser))
            {

                if (Equals(SelectedUser.Username, CurrentUser!.Username))
                {
                    CommonProperties.CurrentUser = SelectedUser;
                }
                
                //UserList = await UserManagementService.GetUserListAsync();
            }
        }
        else
        {
            _messages.ShowWarningNotification("Username & Password cannot be empty");
        }
    }
    #endregion
}