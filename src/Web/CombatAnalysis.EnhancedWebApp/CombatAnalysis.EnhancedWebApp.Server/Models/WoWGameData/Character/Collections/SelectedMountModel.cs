using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

public class SelectedMountModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("source")]
    public WoWGameDataTypeModel Source { get; set; }

    [JsonPropertyName("faction")]
    public WoWGameDataTypeModel? Faction { get; set; }
}
