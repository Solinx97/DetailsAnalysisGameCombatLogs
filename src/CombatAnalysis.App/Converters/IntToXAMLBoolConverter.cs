using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters;

public class IntToXAMLBoolConverter : MvxValueConverter<int, string>
{
    protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
    {
        var sign = string.Empty;
        var compareValue = 0;
        var stringParam = (string)parameter;

        if (!string.IsNullOrWhiteSpace(stringParam))
        {
            var parse = stringParam.Split(':');
            if (parse.Length > 1)
            {
                int.TryParse(parse[0], out compareValue);
                sign = parse[1];
            }
            else if (parse.Length > 0)
            {
                int.TryParse(parse[0], out compareValue);
            }
        }

        string result;
        if (!string.IsNullOrEmpty(sign))
        {
            result = Compare(sign, value, compareValue);
        }
        else
        {
            result = value == compareValue ? "True" : "False";
        }

        return result;
    }

    protected override int ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
    {
        return (int)parameter;
    }

    private string Compare(string sign, int value, int targetNumber)
    {
        var result = string.Empty;

        switch (sign)
        {
            case ">":
                result = value > targetNumber ? "True" : "False";
                break;
            case "<":
                result = value < targetNumber ? "True" : "False";
                break;
            case ">=":
                result = value >= targetNumber ? "True" : "False";
                break;
            case "<=":
                result = value <= targetNumber ? "True" : "False";
                break;
            case "=":
                result = value == targetNumber ? "True" : "False";
                break;
            case "!=":
                result = value != targetNumber ? "True" : "False";
                break;
            default:
                break;
        }

        return result;
    }
}

public class TheNativeIntToXAMLBoolConverter
: MvxNativeValueConverter<IntToXAMLBoolConverter>
{
}
