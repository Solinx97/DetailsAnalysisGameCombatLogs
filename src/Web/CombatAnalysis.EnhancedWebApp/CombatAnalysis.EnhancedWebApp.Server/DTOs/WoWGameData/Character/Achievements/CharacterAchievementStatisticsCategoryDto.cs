namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class CharacterAchievementStatisticsCategoryDto : WoWGameDataEntityDto
{
    public CharacterAchievementStatisticsSubCategoryDto[] SubCategories { get; set; }

    public CharacterAchievementStatisticDto[] Statistics { get; set; }
}
