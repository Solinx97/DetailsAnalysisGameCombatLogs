using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWGameDataPlayableClassModel
{
    [JsonPropertyName("links")]
    public WoWGameDataEntityModel[] Links { get; set; }

    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }
}
