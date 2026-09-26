using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonModeModel
{
    [JsonPropertyName("difficulty")]
    public WoWGameDataTypeModel Difficulty { get; set; }

    [JsonPropertyName("status")]
    public WoWGameDataTypeModel Status { get; set; }

    [JsonPropertyName("progress")]
    public DungeonModeProgressModel Progress { get; set; }
}
