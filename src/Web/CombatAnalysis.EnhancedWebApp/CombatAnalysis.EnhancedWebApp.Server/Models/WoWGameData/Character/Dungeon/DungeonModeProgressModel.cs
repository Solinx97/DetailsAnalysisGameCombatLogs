using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonModeProgressModel
{
    [JsonPropertyName("completed_count")]
    public int CompletedCount { get; set; }

    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    [JsonPropertyName("encounters")]
    public DungeonModeEncountModel[] Encounters { get; set; }
}
