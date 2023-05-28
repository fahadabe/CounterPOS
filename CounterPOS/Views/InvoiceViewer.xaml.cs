namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for InvoiceViewer.xaml
/// </summary>
public partial class InvoiceViewer : Window
{
    public InvoiceViewer(DevExpress.XtraReports.UI.XtraReport xtraReport)
    {
        InitializeComponent();
        viewer.DocumentSource= xtraReport;
        //xtraReport.CreateDocument();
    }
}