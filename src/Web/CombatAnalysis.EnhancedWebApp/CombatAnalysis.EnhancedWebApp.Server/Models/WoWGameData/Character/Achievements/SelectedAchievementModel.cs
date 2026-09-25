using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class SelectedAchievementModel
{
    [JsonPropertyName("category")]
    public AchievementModel Category { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("points")]
    public int Points { get; set; }

    [JsonPropertyName("is_account_wide")]
    public bool IsAccountWide { get; set; }

    [JsonPropertyName("criteria")]
    public SelectedAchievementCriteriaModel Criteria { get; set; }

    [JsonPropertyName("next_achievement")]
    public AchievementModel NextAchievement { get; set; }

    [JsonPropertyName("display_order")]
    public int DisplayOrder { get; set; }
}
