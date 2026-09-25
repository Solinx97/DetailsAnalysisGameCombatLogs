using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneAfixModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
