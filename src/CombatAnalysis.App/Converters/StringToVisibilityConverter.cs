using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace CombatAnalysis.App.Converters
{
    public class StringToVisibilityConverter : MvxValueConverter<string, Visibility>
    {
        protected override Visibility Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = !string.IsNullOrWhiteSpace(value) ? Visibility.Visible : Visibility.Hidden;

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
}