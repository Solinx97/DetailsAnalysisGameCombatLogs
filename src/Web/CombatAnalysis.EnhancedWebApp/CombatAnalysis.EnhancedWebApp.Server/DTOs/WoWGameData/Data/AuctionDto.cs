namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Data;

public class AuctionDto : WoWGameDataEntityDto
{
    public WoWGameDataEntityDto Item { get; set; }

    public long UnitPrice { get; set; }

    public int Quantity { get; set; }

    public string TimeLeft { get; set; }
}
