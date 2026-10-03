using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class SearchItemQualityModel
{
    [JsonPropertyName("name")]
    public SearchItemNameModel Name { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }
}
