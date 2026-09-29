using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.Services;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.Services;

public class WoWAccountService(IWoWGameDataApiClient httpClient, IWoWAccountGameDataApiClient accountHttpClient) : IWoWAccountService
{
    private readonly IWoWGameDataApiClient _httpClient = httpClient;
    private readonly IWoWAccountGameDataApiClient _accountHttpClient = accountHttpClient;

    public async Task<MountModel[]> GetAccountMountsAsync(string regionName, CancellationToken cancellationToken)
    {
        var allMounts = await _httpClient.GetMountsAsync(regionName, cancellationToken);
        var userMounts = await _accountHttpClient.GetMountsAsync(regionName, cancellationToken);

        MountModel[] mounts = [.. allMounts.Mounts.Select(x =>
        {
            var isReceived = userMounts.Mounts.FirstOrDefault(y => y.Mount.Id == x.Id);
            x.IsReceived = isReceived != null;
            return x;
        })];

        return mounts;
    }
}
