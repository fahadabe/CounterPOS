﻿namespace CounterPOS;

public class BrushToColorConverter : IValueConverter
{
    /// <summary>
    /// Converts <see cref="SolidColorBrush"/>  to <see langword="Color"/>.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
            return brush.Color;

        if (value is System.Windows.Media.Color)
            return value;

        // We draw red to visibly see an invalid bind in the UI.
        return new System.Windows.Media.Color { A = 255, R = 255, G = 0, B = 0 };
    }

    /// <summary>
    /// Not Implemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}