using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Identity;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services;

internal class IdentityService : IIdentityService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClientHelper;

    private string _codeVerifier;
    private string _code;
    private HttpListenerService _httpListenerService;

    public IdentityService(IMemoryCache memoryCache, IHttpClientHelper httpClient)
    {
        _memoryCache = memoryCache;
        _httpClientHelper = httpClient;
    }

    public async Task SendAuthorizationRequestAsync(string authorizationRequestType)
    {
        _codeVerifier = PKCEHelper.GenerateCodeVerifier();
        var state = PKCEHelper.GenerateCodeVerifier();
        var codeChallenge = PKCEHelper.GenerateCodeChallenge(_codeVerifier);

        var authorizationUrl = $"{Port.Identity}{authorizationRequestType}?" +
            "grantType=code&" +
            $"clientTd={Authentication.ClientId}&" +
            $"redirectUri={Authentication.RedirectUri}&" +
            $"scope={Authentication.Scope}&" +
            $"state={state}&" +
            "codeChallengeMethod=SHA-256&" +
            $"codeChallenge={codeChallenge}";

        var psi = new ProcessStartInfo
        {
            FileName = authorizationUrl,
            UseShellExecute = true,
        };
        Process.Start(psi);

        _httpListenerService = new HttpListenerService($"{Authentication.Protocol}://{Authentication.Listener}");
        await _httpListenerService.StartListeningAsync(OnCallbackReceived);
    }

    public async Task SendTokenRequestAsync()
    {
        try
        {
            var token = await GetTokenAsync();
            var user= await GetUserAsync(token.AccessToken);
            var customer = await GetCustomerAsync(token.AccessToken, user.Id);

            SetMemoryCache(token.AccessToken, token.RefreshToken, user, customer);
        }
        catch (Exception ex)
        {
        }
    }

    private void OnCallbackReceived(string authorizationCode, string incomingState)
    {
        _code = authorizationCode;
    }

    private async Task<AccessTokenModel> GetTokenAsync()
    {
        var encodedAuthorizationCode = Uri.EscapeDataString(_code);
        var url = $"Token?grantType=authorization_code&clientId={Authentication.ClientId}&codeVerifier={_codeVerifier}&code={encodedAuthorizationCode}&redirectUri={Authentication.RedirectUri}";

        var responseMessage = await _httpClientHelper.GetAsync(url, Port.Identity);
        if (!responseMessage.IsSuccessStatusCode)
        {
            return null;
        }

        var token = await responseMessage.Content.ReadFromJsonAsync<AccessTokenModel>();

        return token;
    }

    private async Task<AppUserModel> GetUserAsync(string accessToken)
    {
        var identityUserId = AccessTokenHelper.GetUserIdFromToken(accessToken);
        if (identityUserId == null)
        {
            return null;
        }

        var response = await _httpClientHelper.GetAsync($"Account/find/{identityUserId}", accessToken, Port.UserApi);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var user = await response.Content.ReadFromJsonAsync<AppUserModel>();

        return user;
    }

    private async Task<CustomerModel> GetCustomerAsync(string accessToken, string userId)
    {
        var response = await _httpClientHelper.GetAsync($"Customer/searchByUserId/{userId}", accessToken, Port.UserApi);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var customer = await response.Content.ReadFromJsonAsync<IEnumerable<CustomerModel>>();
        return customer.FirstOrDefault();
    }

    private void SetMemoryCache(object aceessToken, object refreshToken, object user, object customer)
    {
        _memoryCache.Set(nameof(MemoryCacheValue.AccessToken), refreshToken, new MemoryCacheEntryOptions { Size = 10 });
        _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), refreshToken, new MemoryCacheEntryOptions { Size = 10 });
        _memoryCache.Set(nameof(MemoryCacheValue.User), user, new MemoryCacheEntryOptions { Size = 50 });
        _memoryCache.Set(nameof(MemoryCacheValue.Customer), customer, new MemoryCacheEntryOptions { Size = 50 });
    }
}
