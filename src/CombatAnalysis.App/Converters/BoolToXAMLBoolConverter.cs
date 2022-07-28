using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters
{
    public class BoolToXAMLBoolConverter : MvxValueConverter<bool, string>
    {
        protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = value ? "False" : "True";

            return result;
        }

        protected override bool ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return !string.IsNullOrEmpty(value);
        }
    }

    public class TheNativeBoolToXAMLBoolConverter
         : MvxNativeValueConverter<BoolToXAMLBoolConverter>
    {
    }
}
