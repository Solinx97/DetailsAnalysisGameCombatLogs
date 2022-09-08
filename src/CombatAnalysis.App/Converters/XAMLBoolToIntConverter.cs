using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters
{
    public class XAMLBoolToIntConverter : MvxValueConverter<string, int>
    {
        protected override int Convert(string value, Type targetType, object parameter, CultureInfo culture)
        {
            int.TryParse((string)parameter, out var compareValue);
            var result = value == "True" ? compareValue : 0;

            return result;
        }

        protected override string ConvertBack(int value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)parameter;
        }
    }

    public class TheNativeXAMLBoolToIntConverter
        : MvxNativeValueConverter<XAMLBoolToIntConverter>
    {
    }
}
