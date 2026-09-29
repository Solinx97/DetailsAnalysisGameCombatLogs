using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

public class WoWAccountGameDataApiClient(HttpClient httpClient) : IWoWAccountGameDataApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<WoWAccountRespone> GetCharactersAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/user/wow?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWAccountRespone>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<WoWAccountMountsResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/user/wow/collections/mounts?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWAccountMountsResponse>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<WoWAccountPetsResponse> GetPetsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/user/wow/collections/pets?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<WoWAccountPetsResponse>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }
}
