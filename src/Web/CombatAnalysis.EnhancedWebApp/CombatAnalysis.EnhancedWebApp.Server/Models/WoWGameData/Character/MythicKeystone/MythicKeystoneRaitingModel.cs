using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneRaitingModel
{
    [JsonPropertyName("color")]
    public MythicKeystoneColorModel Color { get; set; }

    [JsonPropertyName("rating")]
    public double Rating { get; set; }
}
