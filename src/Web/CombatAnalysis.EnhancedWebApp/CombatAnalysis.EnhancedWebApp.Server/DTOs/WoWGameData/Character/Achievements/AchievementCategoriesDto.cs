namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class AchievementCategoriesDto
{
    public AchievementCategoryDto[] Categories { get; set; }

    public AchievementCategoryDto[] RootCategories { get; set; }

    public AchievementCategoryDto[] GuildCategories { get; set; }

    public int TotalQuantity { get; set; }

    public int TotalPoints { get; set; }
}
