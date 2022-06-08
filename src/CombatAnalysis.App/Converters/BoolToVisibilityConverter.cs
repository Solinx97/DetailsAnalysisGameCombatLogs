using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace CombatAnalysis.App.Converters
{
    public class BoolToVisibilityConverter : MvxValueConverter<bool, Visibility>
    {
        protected override Visibility Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            bool.TryParse((string)parameter, out var isCollapsed);
            var result = value ? Visibility.Visible : isCollapsed ? Visibility.Collapsed : Visibility.Hidden;

            return result;
        }

        protected override bool ConvertBack(Visibility value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == Visibility.Visible;
        }
    }

    public class TheNativeBoolToVisibilityConverter
        : MvxNativeValueConverter<BoolToVisibilityConverter>
    {
    }
}
