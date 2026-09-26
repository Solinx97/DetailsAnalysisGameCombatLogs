using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWGameDataCurrencyModel
{
    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("display_strings")]
    public WoWGameDataCurrencyDisplayModel DisplayStrings { get; set; }
}
