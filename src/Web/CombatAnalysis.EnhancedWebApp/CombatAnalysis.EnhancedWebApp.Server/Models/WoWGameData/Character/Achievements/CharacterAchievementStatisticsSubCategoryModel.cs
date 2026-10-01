using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementStatisticsSubCategoryModel : WoWGameDataEntityModel
{
    [JsonPropertyName("statistics")]
    public CharacterAchievementStatisticModel[] Statistics { get; set; }
}
