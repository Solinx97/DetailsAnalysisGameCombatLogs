using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Enums;
using CombatAnalysis.Core.Extensions;
using CombatAnalysis.Core.Helpers;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Identity;
using CombatAnalysis.Core.Models.User;
using CombatAnalysis.Core.Services;
using CombatAnalysis.Core.ViewModels.Base;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross.Navigation;
using System.Diagnostics;
using System.Net.Http.Json;

namespace CombatAnalysis.Core.ViewModels.User;

public class AuthorizationViewModel : ParentTemplate
{
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientHelper _httpClientHelper;
    private readonly IMvxNavigationService _mvvmNavigation;

    private HttpListenerService _httpListenerService;

    private string _email;
    private string _password;
    private bool _authIsFailed;
    private bool _serverIsNotAvailable;
    private string _codeVerifier;
    private string _code;

    public AuthorizationViewModel(IMemoryCache memoryCache, IHttpClientHelper httpClient, IMvxNavigationService mvvmNavigation)
    {
        _memoryCache = memoryCache;
        _httpClientHelper = httpClient;
        _mvvmNavigation = mvvmNavigation;

        if (BasicTemplate.Parent is RegistrationViewModel)
        {
            _mvvmNavigation.Close(BasicTemplate.Parent).GetAwaiter().GetResult();
        }

        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "IsRegistrationNotActivated", true);
        BasicTemplate.Parent = this;
    }

    public override void ViewDisappeared()
    {
        BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.IsLoginNotActivated), true);

        base.ViewDisappeared();
    }

    public override void ViewAppeared()
    {
        Task.Run(async () =>
        {
            await SendAuthorizationRequestAsync();
            await SendTokenRequestAsync(_code);
        });
    }

    private async Task SendAuthorizationRequestAsync()
    {
        _codeVerifier = PKCEHelper.GenerateCodeVerifier();
        var state = PKCEHelper.GenerateCodeVerifier();
        var codeChallenge = PKCEHelper.GenerateCodeChallenge(_codeVerifier);

        var authorizationUrl = $"{Port.Identity}authorization?" +
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

    private void OnCallbackReceived(string authorizationCode, string incomingState)
    {
        _code = authorizationCode;
    }

    private async Task SendTokenRequestAsync(string authorizationCode)
    {
        try
        {
            var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
            var url = $"Token?grantType=authorization_code&clientId={Authentication.ClientId}&codeVerifier={_codeVerifier}&code={encodedAuthorizationCode}&redirectUri={Authentication.RedirectUri}";

            var responseMessage = await _httpClientHelper.GetAsync(url, Port.Identity);
            if (!responseMessage.IsSuccessStatusCode)
            {
                return;
            }

            var token = await responseMessage.Content.ReadFromJsonAsync<AccessTokenModel>();
            if (token == null)
            {
                return;
            }

            var identityUserId = AccessTokenHelper.GetUserIdFromToken(token.AccessToken);
            if (identityUserId == null)
            {
                return;
            }

            var response = await _httpClientHelper.GetAsync($"Account/find/{identityUserId}", token.AccessToken, Port.UserApi);
            if (!response.IsSuccessStatusCode)
            {
                return;
            }

            var user = await response.Content.ReadFromJsonAsync<AppUserModel>();
            if (user == null)
            {
                return;
            }

            var customer = await GetCustomerAsync(token.AccessToken, user.Id);
            if (customer == null)
            {
                return;
            }

            SetMemoryCache(token.AccessToken, token.RefreshToken, user, customer);

            _memoryCache.Set(nameof(MemoryCacheValue.AccessToken), token.AccessToken, new MemoryCacheEntryOptions { Size = 10 });
            _memoryCache.Set(nameof(MemoryCacheValue.RefreshToken), token.RefreshToken, new MemoryCacheEntryOptions { Size = 10 });

            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.IsAuth), true);
            BasicTemplate.Handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, nameof(BasicTemplateViewModel.Username), customer.Username);

            await _mvvmNavigation.Close(this);
        }
        catch (Exception ex)
        {
        }
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
