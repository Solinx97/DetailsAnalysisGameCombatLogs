using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;

public class CharacterReputationStandingModel
{
    [JsonPropertyName("raw")]
    public int Raw { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("max")]
    public int Max { get; set; }

    [JsonPropertyName("tier")]
    public int Tier { get; set; }

    [JsonPropertyName("name")]
    public GameDataValueNameModel Name { get; set; }
}
