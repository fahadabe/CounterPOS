namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for BusinessInfoView.xaml
/// </summary>
public partial class BusinessInfoView : UserControl
{
    public BusinessInfoView()
    {
        InitializeComponent();
        var vm = App.GetService<BusinessManagementViewModel>();
        DataContext = vm;
    }
}