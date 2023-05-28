namespace CounterPOS.Converter;

public class MultipleBindingToObjectConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is not null)
        {
            //ProductSizes product = new();
            //decimal qty = 0;
            //product = (ProductSizes)values[0];
            //qty = (values[1] != null && values[1] != DependencyProperty.UnsetValue) ? System.Convert.ToDecimal(values[1]) : 0;
            //object[] toSend = new();
            //toSend[0] = product;
            //toSend[1] = qty;

            //return toSend;
            //var product = (ProductSizes)values[0];
            //var qty = (double)values[1];
            return values.Clone();
        }
        return null;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}