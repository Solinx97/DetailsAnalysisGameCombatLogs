using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;

public class WoWGameDataCharacterModel : WoWGameDataEntityModel
{
    [JsonPropertyName("realm")]
    public WoWRealmModel Realm { get; set; }
}
