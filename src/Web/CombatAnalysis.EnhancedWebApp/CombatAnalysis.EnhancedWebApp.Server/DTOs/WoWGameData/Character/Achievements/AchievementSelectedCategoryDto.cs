namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class AchievementSelectedCategoryDto
{
    public AchievementExtendDto[] Achievements { get; set; }

    public AchievementCategoryDto[] Subcategories { get; set; }

    public bool IsGuildCategory { get; set; }

    public AchievementSelectedCategoryFactionDto AggregatesByFaction { get; set; }

    public int DisplayOrder { get; set; }
}
