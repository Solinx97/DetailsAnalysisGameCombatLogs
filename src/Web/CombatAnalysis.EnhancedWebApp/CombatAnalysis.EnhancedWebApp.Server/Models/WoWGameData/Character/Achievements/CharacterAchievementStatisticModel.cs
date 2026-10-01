using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Achievements;

public class CharacterAchievementStatisticModel : WoWGameDataEntityModel
{
    [JsonPropertyName("last_updated_timestamp")]
    public long LastUpdatedTimestamp { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }
}
