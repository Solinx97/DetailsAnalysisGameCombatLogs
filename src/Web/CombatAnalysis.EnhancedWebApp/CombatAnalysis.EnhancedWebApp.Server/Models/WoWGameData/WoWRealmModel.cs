using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWRealmModel : WoWGameDataEntityModel
{
    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}
