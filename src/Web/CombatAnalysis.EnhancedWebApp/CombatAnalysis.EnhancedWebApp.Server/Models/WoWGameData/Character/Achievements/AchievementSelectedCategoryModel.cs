using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class AchievementSelectedCategoryModel
{
    [JsonPropertyName("achievements")]
    public AchievementExtendModel[] Achievements { get; set; }

    [JsonPropertyName("subcategories")]
    public AchievementCategoryModel[] Subcategories { get; set; }

    [JsonPropertyName("is_guild_category")]
    public bool IsGuildCategory { get; set; }

    [JsonPropertyName("aggregates_by_faction")]
    public AchievementSelectedCategoryFactionModel AggregatesByFaction { get; set; }

    [JsonPropertyName("display_order")]
    public int DisplayOrder { get; set; }
}
