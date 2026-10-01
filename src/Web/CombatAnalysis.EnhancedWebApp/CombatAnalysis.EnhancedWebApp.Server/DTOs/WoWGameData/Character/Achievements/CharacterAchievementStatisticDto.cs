namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class CharacterAchievementStatisticDto : WoWGameDataEntityDto
{
    public DateTimeOffset LastUpdatedTime { get; set; }

    public string? Description { get; set; }

    public double Quantity { get; set; }
}
