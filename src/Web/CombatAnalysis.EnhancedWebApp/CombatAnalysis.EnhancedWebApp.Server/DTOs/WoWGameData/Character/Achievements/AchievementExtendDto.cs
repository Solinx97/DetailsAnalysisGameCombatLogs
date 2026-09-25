namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class AchievementExtendDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset? CompletedTime { get; set; }
}
