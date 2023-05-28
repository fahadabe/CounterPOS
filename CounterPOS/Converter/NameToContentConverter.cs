namespace CounterPOS.Converter;

public class NameToContentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not null)
        {
            Type userControl = Type.GetType(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "." + value, null, null);
            return Activator.CreateInstance(userControl);
        }
        return "";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}