using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWGameDataValueModel
{
    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }
}
