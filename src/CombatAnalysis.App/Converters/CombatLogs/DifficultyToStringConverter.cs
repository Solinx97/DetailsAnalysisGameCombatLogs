using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters.CombatLogs;

public class DifficultyToStringConverter : MvxValueConverter<int, int>
{
    protected override int Convert(int value, Type targetType, object parameter, CultureInfo culture)
    {
        var stringParam = (string)parameter;

        if (stringParam != null)
        {
            var parse = stringParam.Split(':');
            if (parse[0].Contains(value.ToString()))
            {
                return 10;
            }
        }

        return 25;
    }

    protected override int ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == 10 ? 10 : 25;
    }
}

public class TheNativeDifficultyToStringConverter
: MvxNativeValueConverter<DifficultyToStringConverter>
{
}
