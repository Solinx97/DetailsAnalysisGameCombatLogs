using CombatAnalysis.UploadingLogsApp.Consts;
using CombatAnalysis.UploadingLogsApp.Extensions;
using CombatAnalysis.UploadingLogsApp.Helpers;
using CombatAnalysis.UploadingLogsApp.Interfaces;
using CombatAnalysis.UploadingLogsApp.Interfaces.Security;
using CombatAnalysis.UploadingLogsApp.Models.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CombatAnalysis.UploadingLogsApp.Services;

internal class IdentityService(IHttpClientHelper httpClient, ISecurityStorage securityStorage, ILogger<IdentityService> logger) : IIdentityService
{
    private readonly IHttpClientHelper _httpClient = httpClient;
    private readonly ISecurityStorage _securityStorage = securityStorage;
    private readonly ILogger<IdentityService> _logger = logger;

    private string? _codeVerifier;
    private string? _code;
    private HttpListenerService? _httpListenerService;

    public async Task SendAuthorizationRequestAsync(string authorizationRequestType, CancellationToken cancellationToken)
    {
        _codeVerifier = PKCEHelper.GenerateCodeVerifier();
        var state = PKCEHelper.GenerateCodeVerifier();
        var codeChallenge = PKCEHelper.GenerateCodeChallenge(_codeVerifier);
        
        var authorizationUrl = $"{API.Identity}{authorizationRequestType}?" +
            $"client_id={Authentication.ClientId}" +
            $"&redirect_uri={Authentication.RedirectUri}" +
            $"&response_type={AuthenticationGrantType.Code}" +
            $"&scope={Uri.EscapeDataString(Authentication.Scopes)}" +
            $"&state={state}" +
            $"&code_challenge={codeChallenge}" +
            "&code_challenge_method=S256" +
            $"&cancel_uri={Authentication.CancelUri}";

        var psi = new ProcessStartInfo
        {
            FileName = authorizationUrl,
            UseShellExecute = true,
        };
        Process.Start(psi);

        _httpListenerService = new HttpListenerService(_logger);
        await _httpListenerService.StartListeningAsync(Authentication.Listener, OnCallbackReceived, cancellationToken);
    }

    public async Task SendTokenRequestAsync(CancellationToken cancellationToken)
    {
        try
        {
            var token = await GetTokenAsync();
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            await SaveAccountDataAsync(token, cancellationToken);
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }

    private void OnCallbackReceived(string authorizationCode, string incomingState)
    {
        _code = authorizationCode;
    }

    private async Task<TokenResponseModel?> GetTokenAsync()
    {
        try
        {
            ArgumentNullException.ThrowIfNull(_code, nameof(_code));

            var encodedAuthorizationCode = Uri.EscapeDataString(_code);
            var body = new StringContent(
                $"grant_type=authorization_code&client_id={Authentication.ClientId}&code={encodedAuthorizationCode}&redirect_uri={Authentication.RedirectUri}&code_verifier={_codeVerifier}",
                Encoding.UTF8,
                "application/x-www-form-urlencoded"
            );

            _httpClient.BaseAddressApi = string.Empty;
            var response = await _httpClient.PostAsync("connect/token", body, API.Identity);
            response.EnsureSuccessStatusCode();

            var token = await response.Content.ReadFromJsonAsync<TokenResponseModel>();
            ArgumentNullException.ThrowIfNull(token, nameof(token));

            return token;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message);

            return null;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error: {Message}", ex.Message);

            return null;
        }
    }

    private async Task SaveAccountDataAsync(TokenResponseModel token, CancellationToken cancellationToken)
    {
        _securityStorage.SaveAccessToken(token);
        await _securityStorage.GetUserAsync(cancellationToken);
    }
}
