using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using CombatAnalysisIdentity.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class NewPasswordModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;

    public NewPasswordModel(IUserAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public string AppUrl { get; } = Port.Identity;

    public string Email { get; private set; }

    public string Protocol { get; } = Authentication.Protocol;

    public void OnGet(string email)
    {
        Email = email;
    }

    public async Task<IActionResult> OnPostAsync(string email, string password, string confirmPassword)
    {
        if (!password.Equals(confirmPassword))
        {
            ModelState.AddModelError(string.Empty, "Password and confirm password should be equal");

            return Page();
        }

        var (hash, salt) = PasswordHashing.HashPasswordWithSalt(password);

        var identityUser = new IdentityUserModel
        {
            Email = email,
            PasswordHash = hash,
            Salt = salt
        };

        var redirectUri = await _authorizationService.UpdateUserAsync(identityUser);
        if (string.IsNullOrEmpty(redirectUri))
        {
            return Page();
        }

        return Redirect(redirectUri);
    }
}
