using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters;

public class BoolToXAMLBoolConverter : MvxValueConverter<bool, string>
{
    protected override string Convert(bool value, Type targetType, object parameter, CultureInfo culture)
    {
        var isInversion = false;
        var stringParam = (string)parameter;

        if (!string.IsNullOrWhiteSpace(stringParam))
        {
            bool.TryParse(stringParam, out isInversion);
        }

        if (isInversion)
        {
            value = !value;
        }

        var result = value ? "True" : "False";

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
