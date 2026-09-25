using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementsModel
{
    [JsonPropertyName("total_quantity")]
    public int TotalQuantity { get; set; }

    [JsonPropertyName("total_points")]
    public int TotalPoints { get; set; }

    [JsonPropertyName("achievements")]
    public CharacterAchievementModel[] Achievements { get; set; }

    [JsonPropertyName("category_progress")]
    public CharacterAchievementCategoryModel[] CategoryProgress { get; set; }

    [JsonPropertyName("recent_events")]
    public CharacterAchievementRecentEventsModel[] RecentEvents { get; set; }

    [JsonPropertyName("character")]
    public DungeonCharacterModel Character { get; set; }
}
