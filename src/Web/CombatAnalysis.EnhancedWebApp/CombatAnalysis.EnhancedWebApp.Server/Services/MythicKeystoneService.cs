using AutoMapper;
using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

internal class MythicKeystoneService(IWoWGameDataApiClient httpClient, IMapper mapper) : IMythicKeystoneService
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;

    public async Task<MythicKeystoneLeaderboardDto> GetMythicKeystoneLeaderboardAsync(int connectedRealmId, int periodId, string regionName, CancellationToken cancellationToken)
    {
        var periodDungeons = await _httpClient.GetMythicKeystoneLeaderboardAsync(connectedRealmId, regionName, cancellationToken);
        var result = new MythicKeystoneLeaderboardDto();
        foreach (var dungeon in periodDungeons.CurrentLeaderboards)
        {
            var leaderboard = await _httpClient.GetMythicDungeonKeystoneLeaderboardAsync(connectedRealmId, dungeon.Id, periodId, regionName, cancellationToken);
            result.CurrentLeaderboards.TryAdd(dungeon.Name!, _mapper.Map<MythicKeystoneDungeonLeaderboardDto>(leaderboard));
        }

        return result;
    }
}
