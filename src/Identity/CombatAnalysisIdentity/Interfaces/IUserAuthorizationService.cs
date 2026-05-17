using CombatAnalysisIdentity.Models;

namespace CombatAnalysisIdentity.Interfaces;

public interface IUserAuthorizationService
{
    Task<bool> AuthorizationAsync(HttpContext context, string email, string password);

    Task<bool> CreateUserAsync(IdentityUserModel identityUser, AppUserModel appUser, CustomerModel customer);

    Task<bool> CheckIfIdentityUserPresentAsync(string email);

    Task<bool> CheckIfUsernameAlreadyUsedAsync(string username);

    bool IsPasswordStrong(string password);
}
