using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace CombatAnalysis.App.Converters
{
    public class IntToVisibilityConverter : MvxValueConverter<int, Visibility>
    {
        protected override Visibility Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            var isCollapsed = false;
            var targetNumber = 0;
            var stringParam = (string)parameter;

            if (stringParam != null)
            {
                var parse = stringParam.Split(':');
                if (parse.Length > 1)
                {
                    bool.TryParse(parse[0], out isCollapsed);
                    int.TryParse(parse[1], out targetNumber);
                }
                else if (parse.Length > 0)
                {
                    bool.TryParse(parse[0], out isCollapsed);
                }
            }

            var result = value == targetNumber ? Visibility.Visible : isCollapsed ? Visibility.Collapsed : Visibility.Hidden;

            return result;
        }

        protected override int ConvertBack(Visibility value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == Visibility.Visible ? 1 : 0;
        }
    }

    public class TheNativeIntToVisibilityConverter
        : MvxNativeValueConverter<IntToVisibilityConverter>
    {
    }
}
