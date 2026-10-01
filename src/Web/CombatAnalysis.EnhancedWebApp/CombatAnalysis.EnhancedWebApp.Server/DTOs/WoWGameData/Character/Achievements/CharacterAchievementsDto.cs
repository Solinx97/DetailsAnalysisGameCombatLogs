namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Achievements;

public class CharacterAchievementsDto
{
    public int TotalQuantity { get; set; }

    public int TotalPoints { get; set; }

    public CharacterAchievementDto[] Achievements { get; set; }

    public CharacterAchievementCategoryDto[] CategoryProgress { get; set; }

    public CharacterAchievementRecentEventsDto[] RecentEvents { get; set; }

    public WoWGameDataCharacterDto Character { get; set; }
}
