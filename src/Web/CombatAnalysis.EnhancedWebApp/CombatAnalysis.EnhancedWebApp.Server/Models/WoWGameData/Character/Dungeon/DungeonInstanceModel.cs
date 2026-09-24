using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonInstanceModel
{
    [JsonPropertyName("instance")]
    public DungeonNameModel Instance { get; set; }

    [JsonPropertyName("modes")]
    public DungeonModeModel[] Modes { get; set; }
}
