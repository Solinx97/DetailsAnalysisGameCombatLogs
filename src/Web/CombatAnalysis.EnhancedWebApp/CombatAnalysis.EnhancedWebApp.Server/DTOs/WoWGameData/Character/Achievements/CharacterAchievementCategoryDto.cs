namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class CharacterAchievementCategoryDto
{
    public WoWGameDataEntityDto Category { get; set; }

    public int Quantity { get; set; }

    public int Points { get; set; }
}
