using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class DungeonCharacterModel : WoWGameDataEntityModel
{
    [JsonPropertyName("realm")]
    public WoWRealmModel Realm { get; set; }
}
