using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace CombatAnalysis.App.Converters;

public class StringToVisibilityConverter : MvxValueConverter<string, Visibility>
{
    protected override Visibility Convert(string value, Type targetType, object parameter, CultureInfo culture)
    {
        var result = Visibility.Visible;
        if (parameter != null)
        {
            var isEqual = value.Equals(parameter);
            result = !string.IsNullOrWhiteSpace(value)
                ? (isEqual ? Visibility.Visible : Visibility.Hidden)
                : Visibility.Hidden;
        }
        else
        {
            result = !string.IsNullOrWhiteSpace(value) ? Visibility.Visible : Visibility.Hidden;
        }

        return result;
    }

    protected override string ConvertBack(Visibility value, Type targetType, object parameter, CultureInfo culture)
    {
        return value.ToString();
    }
}

public class TheNativeStringToVisibilityConverter
: MvxNativeValueConverter<StringToVisibilityConverter>
{
}