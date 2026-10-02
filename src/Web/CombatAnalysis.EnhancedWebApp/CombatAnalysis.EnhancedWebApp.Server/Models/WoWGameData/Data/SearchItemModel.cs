using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class SearchItemModel
{
    [JsonPropertyName("data")]
    public SearchItemDataModel Data { get; set; }
}
