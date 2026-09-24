using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class GameDataValueNameModel
{
    [JsonPropertyName("ru_RU")]
    public string Name { get; set; }
}
