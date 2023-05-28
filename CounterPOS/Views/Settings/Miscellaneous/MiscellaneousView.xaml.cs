namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for AppearanceView.xaml
/// </summary>
public partial class MiscellaneousView : UserControl
{
    public MiscellaneousView()
    {
        InitializeComponent();
        var vm = App.GetService<MiscellaneousViewModel>();
        DataContext = vm;
    }

    
}