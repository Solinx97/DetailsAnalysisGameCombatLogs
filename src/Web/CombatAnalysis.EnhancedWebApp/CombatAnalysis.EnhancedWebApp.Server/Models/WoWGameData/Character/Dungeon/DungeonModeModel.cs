using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonModeModel
{
    [JsonPropertyName("difficulty")]
    public DungeonModeTypeModel Difficulty { get; set; }

    [JsonPropertyName("status")]
    public DungeonModeTypeModel Status { get; set; }

    [JsonPropertyName("progress")]
    public DungeonModeProgressModel Progress { get; set; }
}
