using CombatAnalysis.EnhancedWebApp.Server.Consts;
using CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients;
using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;
using Microsoft.Extensions.Options;

namespace CombatAnalysis.EnhancedWebApp.Server.HttpClients;

public class WoWGameDataAuthApiClient(HttpClient httpClient, IOptions<BattleNet> battleNet) : IWoWGameDataAuthApiClient
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly BattleNet _battleNet = battleNet.Value;

    public async Task<BattleNetTokenResponse> GetTokenAsync(CancellationToken cancellationToken)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials"
        });

        var responseMessage = await _httpClient.PostAsync("token", content);
        responseMessage.EnsureSuccessStatusCode();

        var result = await responseMessage.Content.ReadFromJsonAsync<BattleNetTokenResponse>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }

    public async Task<BattleNetTokenResponse> AuthorizationCodeExchangeAsync(string authorizationCode, CancellationToken cancellationToken)
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["redirect_uri"] = _battleNet.RedirectUri,
            ["grant_type"] = "authorization_code",
            ["code"] = authorizationCode,
        });

        var responseMessage = await _httpClient.PostAsync("token", content);
        responseMessage.EnsureSuccessStatusCode();

        var result = await responseMessage.Content.ReadFromJsonAsync<BattleNetTokenResponse>();
        return result ?? throw new InvalidOperationException("The WoW API returned an empty response.");
    }
}
