using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class AchievementResponse
{
    [JsonPropertyName("achievements")]
    public WoWGameDataEntityModel[] Achievements { get; set; }
}
