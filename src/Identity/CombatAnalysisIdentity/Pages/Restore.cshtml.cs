using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class RestoreModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;
    private readonly IResetPasswordService _resetPasswordService;

    public RestoreModel(IUserAuthorizationService authorizationService, IResetPasswordService resetPasswordService)
    {
        _authorizationService = authorizationService;
        _resetPasswordService = resetPasswordService;
    }

    public string AppUrl { get; } = Port.Identity;

    public string Protocol { get; } = Authentication.Protocol;

    public async Task<IActionResult> OnPostAsync(string email)
    {
        var isPresent = await _authorizationService.CheckIfIdentityUserPresentAsync(email);
        if (!isPresent)
        {
            ModelState.AddModelError(string.Empty, "User with this Email not present");

            return Page();
        }

        var token = await _resetPasswordService.GeneratePasswordResetTokenAsync(email);

        var redirectUri = Request.Query["redirectUri"];

        return RedirectToPage("newPassword", new { token, email, redirectUri });
    }
}
