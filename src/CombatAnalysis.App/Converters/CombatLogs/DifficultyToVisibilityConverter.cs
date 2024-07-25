using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace CombatAnalysis.App.Converters.CombatLogs;

public class DifficultyToVisibilityConverter : MvxValueConverter<int, Visibility>
{
    protected override Visibility Convert(int value, Type targetType, object parameter, CultureInfo culture)
    {
        var isCollapsed = false;
        var stringParam = (string)parameter;

        if (stringParam != null)
        {
            var parse = stringParam.Split(':');
            bool.TryParse(parse[0], out isCollapsed);
            if (parse[1].Contains(value.ToString()))
            {
                return Visibility.Visible;
            }
        }

        return isCollapsed ? Visibility.Collapsed : Visibility.Hidden;
    }

    protected override int ConvertBack(Visibility value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == Visibility.Visible ? 1 : 0;
    }
}

public class TheNativeDifficultyToVisibilityConverter
    : MvxNativeValueConverter<DifficultyToVisibilityConverter>
{
}
