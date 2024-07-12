using CombatAnalysis.WebApp.Consts;
using CombatAnalysis.WebApp.Extensions;
using CombatAnalysis.WebApp.Helpers;
using CombatAnalysis.WebApp.Interfaces;
using CombatAnalysis.WebApp.Models.Identity;

namespace CombatAnalysis.WebApp.Services;

internal class TokenService : ITokenService
{
    private readonly IHttpClientHelper _httpClient;
    private AccessTokenModel _accessToken;

    public TokenService(IHttpClientHelper httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AccessTokenModel> RefreshAccessTokenAsync(string refreshToken)
    {
        var response = await _httpClient.GetAsync($"Token/refresh?grantType=refresh_token&refreshToken={refreshToken}&clientId={Authentication.ClientId}", Port.Identity);
        if (response.IsSuccessStatusCode)
        {
            var token = await response.Content.ReadFromJsonAsync<AccessTokenModel>();

            return token;
        }
        else
        {
            return null;
        }
    }

    public bool IsAccessTokenCloseToExpiry(string accessToken)
    {
        var validTo = AccessTokenHelper.GetValidToFromToken(accessToken);
        var isCloseToExpiry = DateTime.UtcNow.AddMinutes(5) >= validTo;

        return isCloseToExpiry;
    }
}
