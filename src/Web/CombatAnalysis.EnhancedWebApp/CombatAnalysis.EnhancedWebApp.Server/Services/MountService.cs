using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

public class MountService(IWoWGameDataApiClient httpClient, IWoWUserGameDataApiClient userHttpClient) : IMountService
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IWoWUserGameDataApiClient _userHttpClient = userHttpClient;

    public async Task<MountModel[]> GetUserMountsAsync(string regionName, CancellationToken cancellationToken)
    {
        var allMounts = await _httpClient.GetMountsAsync(regionName, cancellationToken);
        var userMounts = await _userHttpClient.GetMountsAsync(regionName, cancellationToken);

        MountModel[] mounts = [.. allMounts.Mounts.Select(x =>
        {
            var isReceived = userMounts.Mounts.FirstOrDefault(y => y.Mount.Id == x.Id);
            x.IsReceived = isReceived != null;
            return x;
        })];

        return mounts;
    }
}
