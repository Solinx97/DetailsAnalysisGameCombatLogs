using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class ItemSubClassesResponse
{
    [JsonPropertyName("item_subclasses")]
    public WoWGameDataEntityModel[] ItemSubClasses { get; set; }
}
