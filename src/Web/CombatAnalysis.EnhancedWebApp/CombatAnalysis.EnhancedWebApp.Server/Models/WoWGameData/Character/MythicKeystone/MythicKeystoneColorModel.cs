using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneColorModel
{
    [JsonPropertyName("r")]
    public int Red { get; set; }

    [JsonPropertyName("g")]
    public int Green { get; set; }

    [JsonPropertyName("b")]
    public int Blue { get; set; }

    [JsonPropertyName("a")]
    public double Alfa { get; set; }
}
