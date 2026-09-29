using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWMountsResponse
{
    [JsonPropertyName("mounts")]
    public WoWGameDataEntityModel[] Mounts { get; set; }
}
