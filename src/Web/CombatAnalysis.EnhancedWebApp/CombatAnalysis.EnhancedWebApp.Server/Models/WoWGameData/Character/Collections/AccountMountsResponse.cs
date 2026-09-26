using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

public class AccountMountsResponse
{
    [JsonPropertyName("mounts")]
    public AccountMountModel[] Mounts { get; set; }
}
