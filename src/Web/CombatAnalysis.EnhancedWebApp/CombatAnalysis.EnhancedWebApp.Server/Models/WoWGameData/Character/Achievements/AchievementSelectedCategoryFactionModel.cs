using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class AchievementSelectedCategoryFactionModel
{
    [JsonPropertyName("alliance")]
    public AchievementCategoryFactionModel Alliance { get; set; }

    [JsonPropertyName("horde")]
    public AchievementCategoryFactionModel Horde { get; set; }
}
