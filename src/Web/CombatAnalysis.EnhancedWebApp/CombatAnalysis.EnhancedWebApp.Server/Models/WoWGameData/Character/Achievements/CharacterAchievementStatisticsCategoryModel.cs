using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementStatisticsCategoryModel : WoWGameDataEntityModel
{
    [JsonPropertyName("sub_categories")]
    public CharacterAchievementStatisticsSubCategoryModel[] SubCategories { get; set; }

    [JsonPropertyName("statistics")]
    public CharacterAchievementStatisticModel[] Statistics { get; set; }
}
