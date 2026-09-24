using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class FactionModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public GameDataValueNameModel Name { get; set; }
}
