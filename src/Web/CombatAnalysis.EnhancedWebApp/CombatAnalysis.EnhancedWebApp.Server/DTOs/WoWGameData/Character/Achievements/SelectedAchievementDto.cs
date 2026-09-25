namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class SelectedAchievementDto
{
    public AchievementDto Category { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Points { get; set; }

    public bool IsAccountWide { get; set; }

    public SelectedAchievementCriteriaDto Criteria { get; set; }

    public AchievementDto NextAchievement { get; set; }

    public int DisplayOrder { get; set; }
}
