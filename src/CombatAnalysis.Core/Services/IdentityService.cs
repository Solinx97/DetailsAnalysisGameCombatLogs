using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Identity;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.Services;

internal class IdentityService : IIdentityService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly ILogger _logger;

    private string? _codeVerifier;
    private string? _code;
    private HttpListenerService? _httpListenerService;

    public IdentityService(IMemoryCache memoryCache, IHttpClientHelper httpClient, ILogger logger)
    {
        _memoryCache = memoryCache;
        _httpClientHelper = httpClient;
        _logger = logger;
    }

    public async Task SendAuthorizationRequestAsync(string authorizationRequestType)
    {
        _codeVerifier = PKCEHelper.GenerateCodeVerifier();
        var state = PKCEHelper.GenerateCodeVerifier();
        var codeChallenge = PKCEHelper.GenerateCodeChallenge(_codeVerifier);

        var authorizationUrl = $"{Port.Identity}{authorizationRequestType}?" +
            $"grantType={AuthenticationGrantType.Code}&" +
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
            if (token == null)
            {
                return;
            }

            var user = await GetUserAsync(token.AccessToken);

            SetMemoryCache(token.AccessToken, token.RefreshToken, user);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void OnCallbackReceived(string authorizationCode, string incomingState)
    {
        _code = authorizationCode;
    }

    private async Task<AccessTokenModel> GetTokenAsync()
    {
        try
        {
            if (_code == null)
            {
                throw new ArgumentNullException(nameof(_code));
            }

            var encodedAuthorizationCode = Uri.EscapeDataString(_code);
            var url = $"Token?grantType={AuthenticationGrantType.Authorization}&clientId={Authentication.ClientId}&codeVerifier={_codeVerifier}&code={encodedAuthorizationCode}&redirectUri={Authentication.RedirectUri}";

            var response = await _httpClientHelper.GetAsync(url, Port.Identity);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<AccessTokenModel>();
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new AccessTokenModel();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new AccessTokenModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return new AccessTokenModel();
        }
    }

    private async Task<AppUserModel> GetUserAsync(string accessToken)
    {
        try
        {
            var identityUserId = AccessTokenHelper.GetUserIdFromToken(accessToken);
            if (identityUserId == null)
            {
                throw new ArgumentNullException(nameof(identityUserId));
            }

            var response = await _httpClientHelper.GetAsync($"Account/find/{identityUserId}", accessToken, Port.UserApi);
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<AppUserModel>();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return user;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return new AppUserModel();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return new AppUserModel();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return new AppUserModel();
        }
    }

    private void SetMemoryCache(object aceessToken, object refreshToken, object user)
    {
        _memoryCache.Set(nameof(MemoryCacheValue.AccessToken), refreshToken, new MemoryCacheEntryOptions { Size = 10 });
        _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), refreshToken, new MemoryCacheEntryOptions { Size = 10 });
        _memoryCache.Set(nameof(MemoryCacheValue.User), user, new MemoryCacheEntryOptions { Size = 50 });
    }
}
