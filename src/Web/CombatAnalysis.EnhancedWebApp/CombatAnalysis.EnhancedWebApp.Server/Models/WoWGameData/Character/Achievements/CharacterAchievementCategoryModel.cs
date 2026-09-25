using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementCategoryModel
{
    [JsonPropertyName("category")]
    public AchievementModel Category { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("points")]
    public int Points { get; set; }
}
