using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.UserBL.DTO;
using CombatAnalysis.UserBL.Interfaces;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Duende.IdentityModel;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace CombatAnalysisIdentity.Services;

internal class UserAuthorizationService(IMapper mapper, IOptions<Cluster> api, IIdentityUserService identityUserService, 
    ILogger<UserAuthorizationService> logger, IUserService appUserService, ICustomerService customerService,
    IUserTransactionService userTransactionService, IIdentityTransactionService identityTransactionService) : IUserAuthorizationService
{
    private readonly IMapper _mapper = mapper;
    private readonly IIdentityUserService _identityUserService = identityUserService;
    private readonly Cluster _api = api.Value;
    private readonly IUserService _appUserService = appUserService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IUserTransactionService _userTransactionService = userTransactionService;
    private readonly IIdentityTransactionService _identityTransactionService = identityTransactionService;
    private readonly ILogger<UserAuthorizationService> _logger = logger;

    async Task<bool> IUserAuthorizationService.AuthorizationAsync(HttpContext context, string email, string password)
    {
        var user = await _identityUserService.GetByEmailAsync(email);
        if (user == null)
        {
            return false;
        }

        var passwordIsValid = PasswordHashing.VerifyPassword(password, user.PasswordHash, user.Salt);
        if (!passwordIsValid)
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, user.Id),
            new(JwtClaimTypes.Name, user.Email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, IdentityServerConstants.DefaultCookieAuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await context.SignInAsync(IdentityServerConstants.DefaultCookieAuthenticationScheme, claimsPrincipal, new AuthenticationProperties
        {
            IsPersistent = true
        });

        return true;
    }

    async Task<bool> IUserAuthorizationService.CreateUserAsync(IdentityUserModel identityUser, AppUserModel appUser, CustomerModel customer)
    {
        try
        {
            await _userTransactionService.BeginTransactionAsync();
            await _identityTransactionService.BeginTransactionAsync();

            var identityUserMap = _mapper.Map<IdentityUserDto>(identityUser);
            await _identityUserService.CreateAsync(identityUserMap);

            var appUserMap = _mapper.Map<AppUserDto>(appUser);
            await _appUserService.CreateAsync(appUserMap);

            var customerMap = _mapper.Map<CustomerDto>(customer);
            await _customerService.CreateAsync(customerMap);

            await _userTransactionService.CommitTransactionAsync();
            await _identityTransactionService.CommitTransactionAsync();

            return true;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex.Message);

            await _userTransactionService.RollbackTransactionAsync();
            await _identityTransactionService.RollbackTransactionAsync();

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            await _userTransactionService.RollbackTransactionAsync();
            await _identityTransactionService.RollbackTransactionAsync();

            return false;
        }
    }

    async Task<bool> IUserAuthorizationService.CheckIfIdentityUserPresentAsync(string email)
    {
        var userPresent = await _identityUserService.CheckByEmailAsync(email);

        return userPresent;
    }

    async Task<bool> IUserAuthorizationService.CheckIfUsernameAlreadyUsedAsync(string username)
    {
        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{_api.User}api/v1/Account/check/{username}");
            response.EnsureSuccessStatusCode();

            var usernameAlreadyUsed = await response.Content.ReadFromJsonAsync<bool>();
            return usernameAlreadyUsed;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, ex.Message);

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);

            return false;
        }
    }

    bool IUserAuthorizationService.IsPasswordStrong(string password)
    {
        // Check if the password is at least 8 characters long
        if (password.Length < 8)
        {
            return false;
        }

        // Check if the password contains at least one uppercase letter
        if (!Regex.IsMatch(password, "[A-Z]"))
        {
            return false;
        }

        // Check if the password contains at least one lowercase letter
        if (!Regex.IsMatch(password, "[a-z]"))
        {
            return false;
        }

        // Check if the password contains at least one digit
        if (!Regex.IsMatch(password, "[0-9]"))
        {
            return false;
        }

        // Check if the password contains at least one special character
        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
        {
            return false;
        }

        return true;
    }
}
