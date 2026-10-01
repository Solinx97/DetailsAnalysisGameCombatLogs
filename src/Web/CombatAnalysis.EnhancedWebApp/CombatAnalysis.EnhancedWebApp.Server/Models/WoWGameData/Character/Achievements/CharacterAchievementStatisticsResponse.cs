using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementStatisticsResponse
{
    [JsonPropertyName("categories")]
    public CharacterAchievementStatisticsCategoryModel[] Categories { get; set; }
}
