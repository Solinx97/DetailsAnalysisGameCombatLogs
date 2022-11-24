using CombatAnalysis.Core.Enums;
using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters;

public class WhenTypeToStringConverter : MvxValueConverter<WhenType, string>
{
    protected override string Convert(WhenType value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case WhenType.YearAgo:
                return "Вчера";
            case WhenType.MonthAgo:
                return "Вчера";
            case WhenType.WeekAgo:
                return "Вчера";
            case WhenType.Yesterday:
                return "Вчера";
            case WhenType.Today:
                return "Сегодня";
            default:
                return "Неизвестно";
        }
    }

    protected override WhenType ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
    {
        switch (value)
        {
            case nameof(WhenType.Yesterday):
                return WhenType.Yesterday;
            case nameof(WhenType.MonthAgo):
                return WhenType.MonthAgo;
            case nameof(WhenType.WeekAgo):
                return WhenType.WeekAgo;
            case nameof(WhenType.Today):
                return WhenType.Today;
            default:
                return WhenType.Today;
        }
    }
}

public class TheNativeWhenTypeToStringConverter
: MvxNativeValueConverter<WhenTypeToStringConverter>
{
}
