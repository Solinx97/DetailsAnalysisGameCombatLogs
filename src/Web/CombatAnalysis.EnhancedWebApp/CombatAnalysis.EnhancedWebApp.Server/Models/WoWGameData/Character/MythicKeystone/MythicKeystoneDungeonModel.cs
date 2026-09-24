using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonModel
{
    [JsonPropertyName("name")]
    public GameDataValueNameModel Name { get; set; }
}
