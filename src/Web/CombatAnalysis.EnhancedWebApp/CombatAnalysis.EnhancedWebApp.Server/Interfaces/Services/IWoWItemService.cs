using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Data;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

public interface IWoWItemService
{
    Task<AuctionDto[]> GetAuctionAsync(string regionName, int itemId, CancellationToken cancellationToken);
}
