namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for SaleReportView.xaml
/// </summary>
public partial class SaleReportView : UserControl
{
    public SaleReportView()
    {
        InitializeComponent();
        var vm = App.GetService<SaleReportViewModel>();
        DataContext = vm;
    }
}