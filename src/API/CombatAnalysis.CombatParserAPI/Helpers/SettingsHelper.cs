using System.Text.Json;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal static class SettingsHelper
{
    public static Dictionary<string, string>? ConvertToDictionary(string? jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            return new Dictionary<string, string>();
        }

        return JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
    }
}
