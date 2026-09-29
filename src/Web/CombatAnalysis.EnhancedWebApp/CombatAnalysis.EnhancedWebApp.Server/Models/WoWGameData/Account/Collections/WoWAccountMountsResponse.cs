using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountMountsResponse
{
    [JsonPropertyName("mounts")]
    public WoWAccountMountModel[] Mounts { get; set; }
}
