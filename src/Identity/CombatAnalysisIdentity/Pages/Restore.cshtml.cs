using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class RestoreModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;

    public RestoreModel(IUserAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
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

        var redirectUri = $"{AppUrl}newPassword?state={email}";

        return RedirectToPage("NewPassword", new { email });
    }
}
