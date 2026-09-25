using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonModeEncountModel
{
    [JsonPropertyName("encounter")]
    public DungeonModel Encounter { get; set; }

    [JsonPropertyName("completed_count")]
    public int CompletedCount { get; set; }

    [JsonPropertyName("last_kill_timestamp")]
    public long LastKillTimestamp { get; set; }
}
