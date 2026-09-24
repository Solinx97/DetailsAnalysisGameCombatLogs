using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneSeasonModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
}
