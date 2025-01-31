using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class NewPasswordModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;
    private readonly IUserVerification _userVerification;

    public NewPasswordModel(IUserAuthorizationService authorizationService, IUserVerification userVerification)
    {
        _authorizationService = authorizationService;
        _userVerification = userVerification;
    }

    public string AppUrl { get; } = API.Identity;

    [BindProperty]
    public PasswordResetModel PasswordReset{ get; set; }

    public string Protocol { get; } = Authentication.Protocol;

    public IActionResult OnGet(string token)
    {
        PasswordReset = new PasswordResetModel
        {
            Token = token,
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Confirm password should be equal Password");

            return Page();
        }

        var passwordIsStrong = _authorizationService.IsPasswordStrong(PasswordReset.Password);
        if (!passwordIsStrong)
        {
            ModelState.AddModelError(string.Empty, "Password should have at least 8 characters, upper/lowercase character, digit and special symbol");

            return Page();
        }

        var wasReseted = await _userVerification.ResetPasswordAsync(PasswordReset.Token, PasswordReset.Password);
        if (wasReseted)
        {
            var redirectUri = $"{Authentication.Protocol}://{Request.Query["redirectUri"]}?accessRestored=true";

            return Redirect(redirectUri);
        }

        return Page();
    }
}
