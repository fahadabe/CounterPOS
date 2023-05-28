namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for ItemReportView.xaml
/// </summary>
public partial class ItemReportView : UserControl
{
    public ItemReportView()
    {
        InitializeComponent();
        var vm = App.GetService<ItemReportViewModel>();
        DataContext = vm;
    }
}