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
            var isCollapsed = false;
            var isInversion = false;
            var stringParam = (string)parameter;

            if (stringParam != null)
            {
                var parse = stringParam.Split(':');
                if (parse.Length > 1)
                {
                    bool.TryParse(parse[0], out isCollapsed);
                    bool.TryParse(parse[1], out isInversion);
                }
                else if (parse.Length > 0)
                {
                    bool.TryParse(parse[0], out isCollapsed);
                }
            }

            if (isInversion)
            {
                value = !value;
            }

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
