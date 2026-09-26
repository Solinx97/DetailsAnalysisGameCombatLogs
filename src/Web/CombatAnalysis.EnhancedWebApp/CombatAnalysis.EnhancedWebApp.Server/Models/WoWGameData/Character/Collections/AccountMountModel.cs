using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

public class AccountMountModel
{
    [JsonPropertyName("mount")]
    public MountModel Mount { get; set; }

    [JsonPropertyName("is_useable")]
    public bool IsUseable { get; set; }
}
