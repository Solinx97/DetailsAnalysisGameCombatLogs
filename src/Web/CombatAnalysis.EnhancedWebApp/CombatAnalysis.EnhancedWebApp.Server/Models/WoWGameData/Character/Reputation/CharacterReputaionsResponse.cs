using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Reputation;

public class CharacterReputaionsResponse
{
    [JsonPropertyName("reputations")]
    public CharacterReputationModel[] Reputations { get; set; }
}
