using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;

public class CharacterReputationModel
{
    [JsonPropertyName("faction")]
    public CharacterReputationFactionModel Faction { get; set; }

    [JsonPropertyName("standing")]
    public CharacterReputationStandingModel Standing { get; set; }
}
