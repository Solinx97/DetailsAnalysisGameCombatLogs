using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

public class CharacterMountModel
{
    [JsonPropertyName("mount")]
    public WoWMountModel Mount { get; set; }

    [JsonPropertyName("is_useable")]
    public bool IsUseable { get; set; }
}
