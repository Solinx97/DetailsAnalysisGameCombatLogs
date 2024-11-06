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
    private readonly IResetPasswordService _resetPasswordService;

    public NewPasswordModel(IUserAuthorizationService authorizationService, IResetPasswordService resetPasswordService)
    {
        _authorizationService = authorizationService;
        _resetPasswordService = resetPasswordService;
    }

    public string AppUrl { get; } = Port.Identity;

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

    public async Task<IActionResult> OnPostAsync(string token)
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

        var wasReseted = await _resetPasswordService.ResetPasswordAsync(token, PasswordReset.Password);
        if (wasReseted)
        {
            var redirectUri = $"{Authentication.Protocol}://{Request.Query["redirectUri"]}?accessRestored=true";

            return Redirect(redirectUri);
        }

        return Page();
    }
}
