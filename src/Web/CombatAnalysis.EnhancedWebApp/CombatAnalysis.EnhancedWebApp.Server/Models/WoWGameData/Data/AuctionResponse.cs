using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Data;

public class AuctionResponse
{
    [JsonPropertyName("auctions")]
    public AuctionModel[] Auctions { get; set; }
}
