using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Data;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

internal class WoWItemService(IWoWGameDataApiClient httpClient, IMapper mapper) : IWoWItemService
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;

    public async Task<AuctionDto[]> GetAuctionAsync(string regionName, int itemId, CancellationToken cancellationToken)
    {
        var auctions = await _httpClient.GetAuctionHouseCommoditiesAsync(regionName, cancellationToken);
        var findAuctions = auctions.Auctions.Where(x => x.Item.Id == itemId).OrderBy(x => x.UnitPrice).ToArray();
        var map = _mapper.Map<AuctionDto[]>(findAuctions);

        return map;
    }
}
