using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class DungeonCharacterModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("realm")]
    public RealmModel Realm { get; set; }
}
