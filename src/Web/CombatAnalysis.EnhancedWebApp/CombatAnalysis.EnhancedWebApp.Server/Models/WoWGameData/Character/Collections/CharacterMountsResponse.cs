using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

public class CharacterMountsResponse
{
    [JsonPropertyName("mounts")]
    public CharacterMountModel[] Mounts { get; set; }
}
