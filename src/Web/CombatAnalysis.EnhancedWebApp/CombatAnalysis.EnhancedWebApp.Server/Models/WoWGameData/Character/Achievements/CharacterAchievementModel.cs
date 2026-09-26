using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementModel
{
    [JsonPropertyName("achievement")]
    public WoWGameDataEntityModel Achievement { get; set; }

    [JsonPropertyName("criteria")]
    public CharacterAchievementCriteriaModel Criteria { get; set; }

    [JsonPropertyName("completed_timestamp")]
    public long CompletedTimestamp { get; set; }
}
