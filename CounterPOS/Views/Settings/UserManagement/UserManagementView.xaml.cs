namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for UserManagementView.xaml
/// </summary>
public partial class UserManagementView : UserControl
{
    public UserManagementView()
    {
        InitializeComponent();
        var vm = App.GetService<UserManagementViewModel>();
        DataContext = vm;
    }
}