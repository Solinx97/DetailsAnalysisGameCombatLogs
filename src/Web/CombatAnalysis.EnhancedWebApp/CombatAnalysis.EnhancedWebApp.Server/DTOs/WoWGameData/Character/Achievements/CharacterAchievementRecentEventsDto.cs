namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class CharacterAchievementRecentEventsDto
{
    public WoWGameDataEntityDto Achievement { get; set; }

    public DateTimeOffset Time { get; set; }
}
