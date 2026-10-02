using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class AuctionModel : WoWGameDataEntityModel
{
    [JsonPropertyName("item")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("unit_price")]
    public long UnitPrice { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("time_left")]
    public string TimeLeft { get; set; }
}
