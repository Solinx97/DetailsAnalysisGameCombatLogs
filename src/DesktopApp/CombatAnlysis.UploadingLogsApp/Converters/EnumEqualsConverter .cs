using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.UploadingLogsApp.Converters;

public class EnumEqualsConverter : IValueConverter
{
    public object Convert(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        if (value is null || parameter is null)
            return false;

        return value.ToString() == parameter.ToString();
    }

    public object ConvertBack(
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}
