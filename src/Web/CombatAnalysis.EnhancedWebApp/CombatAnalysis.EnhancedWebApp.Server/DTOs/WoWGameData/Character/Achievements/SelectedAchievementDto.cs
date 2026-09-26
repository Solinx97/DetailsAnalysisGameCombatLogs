namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class SelectedAchievementDto
{
    public WoWGameDataEntityDto Category { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Points { get; set; }

    public bool IsAccountWide { get; set; }

    public SelectedAchievementCriteriaDto Criteria { get; set; }

    public WoWGameDataEntityDto NextAchievement { get; set; }

    public int DisplayOrder { get; set; }
}
