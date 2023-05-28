namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for PaymentTypeWindow.xaml
/// </summary>
public partial class PaymentTypeWindow : Window
{
    public PaymentTypeWindow(PaymentTypeViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}