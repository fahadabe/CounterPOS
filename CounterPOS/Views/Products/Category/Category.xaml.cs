namespace CounterPOS.Views.Category;

/// <summary>
/// Interaction logic for Category.xaml
/// </summary>
public partial class Category : Window
{
    public Category(CategoryViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}