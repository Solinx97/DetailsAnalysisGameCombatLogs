using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters
{
    public class IntToXAMLBoolConverter : MvxValueConverter<int, string>
    {
        protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            int.TryParse((string)parameter, out var step);
            var result = value == step ? "True" : "False";

            return result;
        }

        protected override int ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)parameter;
        }
    }

    public class TheNativeIntToXAMLBoolConverter
    : MvxNativeValueConverter<IntToXAMLBoolConverter>
    {
    }
}
