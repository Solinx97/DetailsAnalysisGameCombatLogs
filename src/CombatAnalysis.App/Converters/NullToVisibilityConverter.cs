using MvvmCross.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace CombatAnalysis.App.Converters;

public class NullToVisibilityConverter : MvxValueConverter
{
    public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? Visibility.Hidden : Visibility.Visible;
    }

    public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Visibility.Hidden;
    }
}

public class TheNativeNullToVisibilityConverter
    : MvxValueConverter<NullToVisibilityConverter>
{
}