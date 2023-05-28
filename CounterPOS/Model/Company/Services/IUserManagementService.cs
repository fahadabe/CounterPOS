namespace CounterPOS.Model;

public interface IUserManagementService
{
    Task AddUserAsync(UserModel ObjNewUser);
    bool CheckIfUserExist(string username);
    Task DeleteUserAsync(UserModel ObjDeleteUser);
    Task<ObservableCollection<UserModel>> GetUserListAsync();
    Task<bool> UpdateUserAsync(UserModel ObjUpdateUser);
}