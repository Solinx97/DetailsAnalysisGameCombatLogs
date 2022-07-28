using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using System;
using System.Globalization;

namespace CombatAnalysis.App.Converters
{
    public class IntToXAMLExpressConverter : MvxValueConverter<int, string>
    {
        protected override string Convert(int value, Type targetType, object parameter, CultureInfo culture)
        {
            var data = (string)parameter;
            var split = data.Split(',');
            int.TryParse(split[1], out var step);

            var result = string.Empty;
            switch (split[0])
            {
                case ">":
                    result = value > step ? "True" : "False";
                    break;
                case "<":
                    result = value < step ? "True" : "False";
                    break;
                case ">=":
                    result = value >= step ? "True" : "False";
                    break;
                case "<=":
                    result = value <= step ? "True" : "False";
                    break;
                case "=":
                    result = value == step ? "True" : "False";
                    break;
                default:
                    break;
            }

            return result;
        }

        protected override int ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)parameter;
        }
    }

    public class TheNativeIntToXAMLExpressConverter
        : MvxNativeValueConverter<IntToXAMLExpressConverter>
    {
    }
}
