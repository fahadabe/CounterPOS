namespace CounterPOS.Views;

/// <summary>
/// Interaction logic for ExpanseCategory.xaml
/// </summary>
public partial class ExpanseCategory : Window
{
    public ExpanseCategory(ExpanseCategoryViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}