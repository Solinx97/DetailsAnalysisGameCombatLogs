namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class CharacterAchievementDto
{
    public WoWGameDataEntityDto Achievement { get; set; }

    public CharacterAchievementCriteriaDto Criteria { get; set; }

    public DateTimeOffset CompletedTime { get; set; }
}
