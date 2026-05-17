using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CombatAnalysis.App.Converters;

public class EqualsToVisibilityConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] == null || values[1] == null)
            return Visibility.Collapsed;

        return values[0].ToString() == values[1].ToString()
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
