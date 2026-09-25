using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class AchievementCategoriesModel
{
    [JsonPropertyName("categories")]
    public AchievementCategoryModel[] Categories { get; set; }

    [JsonPropertyName("root_categories")]
    public AchievementCategoryModel[] RootCategories { get; set; }

    [JsonPropertyName("guild_categories")]
    public AchievementCategoryModel[] GuildCategories { get; set; }

    public int TotalQuantity { get; set; }

    public int TotalPoints { get; set; }
}
