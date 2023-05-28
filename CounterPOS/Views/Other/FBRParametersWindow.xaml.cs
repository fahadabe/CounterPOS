namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for Category.xaml
/// </summary>
public partial class FBRParametersWindow : Window
{
    public FBRParametersWindow(FBRParametersViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}