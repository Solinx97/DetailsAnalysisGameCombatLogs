using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class SearchItemNameModel
{
    [JsonPropertyName("it_IT")]
    public string IT { get; set; }

    [JsonPropertyName("ru_RU")]
    public string RU { get; set; }

    [JsonPropertyName("en_GB")]
    public string EN { get; set; }

    [JsonPropertyName("en_US")]
    public string US { get; set; }

    [JsonPropertyName("es_MX")]
    public string MX { get; set; }

    [JsonPropertyName("es_ES")]
    public string ES { get; set; }

    [JsonPropertyName("de_DE")]
    public string DE { get; set; }
}
