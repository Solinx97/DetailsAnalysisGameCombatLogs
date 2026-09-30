using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class SelectedWoWAccountToyItemModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("item")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("source_description")]
    public string Description { get; set; }

    [JsonPropertyName("source")]
    public WoWGameDataTypeModel Source { get; set; }
}
