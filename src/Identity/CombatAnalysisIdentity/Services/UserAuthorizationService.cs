using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using CombatAnalysisIdentity.Security;

namespace CombatAnalysisIdentity.Services;

internal class UserAuthorizationService : IUserAuthorizationService
{
    private readonly IMapper _mapper;
    private readonly IOAuthCodeFlowService _oAuthCodeFlowService;
    private readonly IIdentityUserService _identityUserService;
    private readonly ILogger<UserAuthorizationService> _logger;
    private AuthorizationRequestModel _authorizationRequest = new AuthorizationRequestModel();

    public UserAuthorizationService(IMapper mapper, IOAuthCodeFlowService oAuthCodeFlowService, IIdentityUserService identityUserService, ILogger<UserAuthorizationService> logger)
    {
        _mapper = mapper;
        _oAuthCodeFlowService = oAuthCodeFlowService;
        _identityUserService = identityUserService;
        _logger = logger;
    }

    public async Task<string> AuthorizationAsync(HttpRequest request, string email, string password)
    {
        var user = await _identityUserService.GetAsync(email);
        if (user == null)
        {
            return string.Empty;
        }

        var passwordIsValid = PasswordHashing.VerifyPassword(password, user.PasswordHash, user.Salt);
        if (!passwordIsValid)
        {
            return string.Empty;
        }

        GetAuthorizationRequestData(request);

        var authorizationCode = await _oAuthCodeFlowService.GenerateAuthorizationCodeAsync(user.Id, _authorizationRequest.ClientTd, _authorizationRequest.CodeChallenge, _authorizationRequest.CodeChallengeMethod, _authorizationRequest.RedirectUri);

        var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
        var redirectUrl = $"{Authentication.Protocol}://{_authorizationRequest.RedirectUri}?code={encodedAuthorizationCode}&state={_authorizationRequest.State}";

        return redirectUrl;
    }

    public async Task<bool> ClientValidationAsync(HttpRequest request)
    {
        GetAuthorizationRequestData(request);

        var clientIsValid = await _oAuthCodeFlowService.ValidateClientAsync(_authorizationRequest.ClientTd, _authorizationRequest.RedirectUri, _authorizationRequest.Scope);

        return clientIsValid;
    }

    public async Task<bool> CreateUserAsync(IdentityUserModel identityUser, AppUserModel appUser, CustomerModel customer)
    {
        var identityUserMap = _mapper.Map<IdentityUserDto>(identityUser);
        await _identityUserService.CreateAsync(identityUserMap);

        var httpClient = new HttpClient();
        var responseMessage = await httpClient.PostAsync($"{Port.UserApi}api/v1/Account", JsonContent.Create(appUser));
        if (responseMessage.IsSuccessStatusCode)
        {
            var response = await responseMessage.Content.ReadFromJsonAsync<AppUserModel>();

            customer.AppUserId = response.Id ?? string.Empty;
        }
        else
        {
            return false;
        }

        responseMessage = await httpClient.PostAsync($"{Port.UserApi}api/v1/Customer", JsonContent.Create(customer));

        return responseMessage.IsSuccessStatusCode;
    }

    public async Task<bool> CheckIfIdentityUserPresentAsync(string email)
    {
        var userPresent = await _identityUserService.CheckByEmailAsync(email);

        return userPresent;
    }

    public async Task<bool> CheckIfUsernameAlreadyUsedAsync(string username)
    {
        try
        {
            var httpClient = new HttpClient();
            var responseMessage = await httpClient.GetAsync($"{Port.UserApi}api/v1/Account/check/{username}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var usernameAlreadyUsed = await responseMessage.Content.ReadFromJsonAsync<bool>();
                return usernameAlreadyUsed;
            }
            else
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return true;
        }
    }

    private void GetAuthorizationRequestData(HttpRequest request)
    {
        if (request.Query.TryGetValue(AuthorizationRequest.RedirectUri.ToString(), out var redirectUri))
        {
            _authorizationRequest.RedirectUri = redirectUri;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.GrantType.ToString(), out var grantType))
        {
            _authorizationRequest.GrantType = grantType;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.ClientTd.ToString(), out var clientTd))
        {
            _authorizationRequest.ClientTd = clientTd;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.Scope.ToString(), out var scope))
        {
            _authorizationRequest.Scope = scope;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.State.ToString(), out var state))
        {
            _authorizationRequest.State = state;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.CodeChallengeMethod.ToString(), out var codeChallengeMethod))
        {
            _authorizationRequest.CodeChallengeMethod = codeChallengeMethod;
        }

        if (request.Query.TryGetValue(AuthorizationRequest.CodeChallenge.ToString(), out var codeChallenge))
        {
            _authorizationRequest.CodeChallenge = codeChallenge;
        }
    }
}
