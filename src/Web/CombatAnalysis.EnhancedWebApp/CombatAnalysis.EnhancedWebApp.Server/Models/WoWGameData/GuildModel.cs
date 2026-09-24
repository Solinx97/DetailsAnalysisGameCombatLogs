using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class GuildModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("realm")]
    public RealmModel Realm { get; set; }

    [JsonPropertyName("faction")]
    public FactionModel Faction { get; set; }
}
