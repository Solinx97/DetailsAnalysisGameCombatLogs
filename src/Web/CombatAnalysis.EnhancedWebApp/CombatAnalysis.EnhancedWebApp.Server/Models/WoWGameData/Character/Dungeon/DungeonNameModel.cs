using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonNameModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
