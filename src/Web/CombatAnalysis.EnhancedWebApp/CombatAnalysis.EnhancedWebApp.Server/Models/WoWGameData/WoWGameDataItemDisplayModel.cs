using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWGameDataItemDisplayModel
{
    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }

    [JsonPropertyName("color")]
    public WoWGameDataColorModel Color { get; set; }
}
