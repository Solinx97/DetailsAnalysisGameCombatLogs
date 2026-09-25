using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementCriteriaModel
{
    [JsonPropertyName("is_completed")]
    public bool IsCompleted { get; set; }
}
