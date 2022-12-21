using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters;

public class StringToXAMLBoolConverter : MvxValueConverter<string, string>
{
    protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
    {
        var result = !string.IsNullOrWhiteSpace(value) ? "True" : "False";

        return result;
    }

    protected override string ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }
}

public class TheNativeStringToXAMLBoolConverter
    : MvxNativeValueConverter<StringToXAMLBoolConverter>
{
}
