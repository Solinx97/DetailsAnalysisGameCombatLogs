using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

public class WoWUserGameDataApiClient(HttpClient httpClient) : IWoWUserGameDataApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CharacterMountsResponse> GetMountsAsync(string regionName, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"profile/user/wow/collections/mounts?namespace=profile-{regionName}&locale={WoWDataLocale.Locale}", cancellationToken);

        var result = await response.Content.ReadFromJsonAsync<CharacterMountsResponse>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }
}
