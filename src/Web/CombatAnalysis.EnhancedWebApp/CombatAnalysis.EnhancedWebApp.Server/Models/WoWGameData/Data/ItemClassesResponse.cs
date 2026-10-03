using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class ItemClassesResponse
{
    [JsonPropertyName("item_classes")]
    public WoWGameDataEntityModel[] ItemClasses { get; set; }
}
