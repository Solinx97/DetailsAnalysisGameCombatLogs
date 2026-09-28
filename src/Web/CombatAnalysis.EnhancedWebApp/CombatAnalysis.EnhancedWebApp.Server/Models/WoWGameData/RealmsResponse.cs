using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class RealmsResponse
{
    [JsonPropertyName("realms")]
    public WoWRealmModel[] Realms { get; set; }
}
