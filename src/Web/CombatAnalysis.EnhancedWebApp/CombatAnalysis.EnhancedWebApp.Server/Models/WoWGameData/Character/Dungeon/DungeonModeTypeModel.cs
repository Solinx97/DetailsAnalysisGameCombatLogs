using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class DungeonModeTypeModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public GameDataValueNameModel Name { get; set; }
}
