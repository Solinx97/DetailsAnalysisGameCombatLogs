using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWGameDataValueNameModel
{
    [JsonPropertyName("ru_RU")]
    public string Name { get; set; }
}
