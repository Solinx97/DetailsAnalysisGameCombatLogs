using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class SelectedWoWAccountCollectionItemModel : WoWGameDataEntityModel
{
    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("source")]
    public WoWGameDataTypeModel Source { get; set; }

    [JsonPropertyName("faction")]
    public WoWGameDataTypeModel? Faction { get; set; }
}
