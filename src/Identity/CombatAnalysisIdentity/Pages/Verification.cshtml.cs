using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class VerificationModel : PageModel
{
    private readonly IUserVerification _userVerification;

    public VerificationModel(IUserVerification userVerification)
    {
        _userVerification = userVerification;
    }

    public async Task<IActionResult> OnGetAsync(string token)
    {
        var wasVerified = await _userVerification.VerifyEmailAsync(token);
        if (wasVerified)
        {
            var redirectUri = $"{Authentication.Protocol}://{Request.Query["redirectUri"]}?verified=true";

            return Redirect(redirectUri);
        }

        return Page();
    }
}
