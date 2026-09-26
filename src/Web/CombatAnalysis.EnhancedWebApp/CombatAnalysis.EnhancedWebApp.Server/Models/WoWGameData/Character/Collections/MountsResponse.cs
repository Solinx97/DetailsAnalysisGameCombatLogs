using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

public class MountsResponse
{
    [JsonPropertyName("mounts")]
    public MountModel[] Mounts { get; set; }
}
