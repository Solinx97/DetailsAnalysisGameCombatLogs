namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class CharacterAchievementRecentEventsDto
{
    public AchievementDto Achievement { get; set; }

    public DateTimeOffset Time { get; set; }
}
