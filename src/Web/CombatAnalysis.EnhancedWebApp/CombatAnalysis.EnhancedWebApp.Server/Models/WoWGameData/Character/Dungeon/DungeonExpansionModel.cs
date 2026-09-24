using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonExpansionModel
{
    [JsonPropertyName("expansion")]
    public DungeonNameModel Expansion { get; set; }

    [JsonPropertyName("instances")]
    public DungeonInstanceModel[] Instances { get; set; }
}
