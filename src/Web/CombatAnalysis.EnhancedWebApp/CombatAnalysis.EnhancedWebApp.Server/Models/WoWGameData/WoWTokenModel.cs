using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWTokenModel
{
    [JsonPropertyName("last_updated_timestamp")]
    public long LastUpdatedTimestamp { get; set; }

    [JsonPropertyName("price")]
    public long Price { get; set; }
}
