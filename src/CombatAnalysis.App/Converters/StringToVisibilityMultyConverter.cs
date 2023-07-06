using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatAnalysis.App.Converters;

public class StringToVisibilityMultyConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var result = Visibility.Visible;
        var isInversion = false;
        if (parameter != null)
        {
            bool.TryParse((string)parameter, out isInversion);
        }

        if (values.Length == 2)
        {
            result = values[0].Equals(values[1]) ? Visibility.Visible : Visibility.Collapsed;
            if (isInversion)
            {
                result = result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        return result;
    }

    public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
    {
        return new object[0];
    }
}
