using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class WoWGameDataCurrencyDisplayModel
{
    [JsonPropertyName("header")]
    public string Header { get; set; }

    [JsonPropertyName("gold")]
    public string Gold { get; set; }

    [JsonPropertyName("silver")]
    public string Silver { get; set; }

    [JsonPropertyName("copper")]
    public string Copper { get; set; }
}
