namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for PurchaseReportView.xaml
/// </summary>
public partial class PurchaseReportView : UserControl
{
    public PurchaseReportView()
    {
        InitializeComponent();
        var vm = App.GetService<PurchaseReportViewModel>();
        DataContext = vm;
    }
}