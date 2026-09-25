using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementRecentEventsModel
{
    [JsonPropertyName("achievement")]
    public AchievementModel Achievement { get; set; }

    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
}
