using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;

public class CharacterModel : WoWGameDataEntityModel
{
    [JsonPropertyName("realm")]
    public RealmModel Realm { get; set; }

    [JsonPropertyName("playable_class")]
    public WoWGameDataEntityModel PlayableClass { get; set; }

    [JsonPropertyName("playable_race")]
    public WoWGameDataEntityModel PlayableRace { get; set; }

    [JsonPropertyName("gender")]
    public WoWGameDataTypeModel Gender { get; set; }

    [JsonPropertyName("faction")]
    public WoWGameDataTypeModel Faction { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }
}
