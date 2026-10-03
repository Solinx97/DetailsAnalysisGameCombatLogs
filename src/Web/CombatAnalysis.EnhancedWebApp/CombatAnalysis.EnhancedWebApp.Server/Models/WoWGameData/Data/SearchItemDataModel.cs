using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class SearchItemDataModel : WoWGameDataEntityModel
{
    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("required_level")]
    public int RequiredLevel { get; set; }

    [JsonPropertyName("sell_price")]
    public long SellPrice { get; set; }

    [JsonPropertyName("item_class")]
    public SearchItemClassModel ItemClass { get; set; }

    [JsonPropertyName("item_subclass")]
    public SearchItemClassModel ItemSubclass { get; set; }

    [JsonPropertyName("quality")]
    public SearchItemQualityModel Quality { get; set; }

    [JsonPropertyName("is_equippable")]
    public bool IsEquippable { get; set; }

    [JsonPropertyName("purchase_quantity")]
    public int PurchaseQuantity { get; set; }

    [JsonPropertyName("max_count")]
    public int MaxCount { get; set; }

    [JsonPropertyName("is_stackable")]
    public bool IsStackable { get; set; }

    [JsonPropertyName("name")]
    public SearchItemNameModel Name { get; set; }

    [JsonPropertyName("purchase_price")]
    public long PurchasePrice { get; set; }
}
