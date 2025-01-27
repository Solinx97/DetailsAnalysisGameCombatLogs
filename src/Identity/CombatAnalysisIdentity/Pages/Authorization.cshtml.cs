using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class AuthorizationModel(IUserAuthorizationService authorizationService) : PageModel
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;

    public bool QueryIsValid { get; set; }

    public string AppUrl { get; } = API.Identity;

    public string Protocol { get; } = Authentication.Protocol;

    [BindProperty]
    public AuthorizationDataModel? Authorization { get; set; }

    public async Task OnGetAsync()
    {
        await RequestValidationAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        await RequestValidationAsync();

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return Page();
        }

        if (Authorization == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt");

            return Page();
        }

        var redirectUri = await _authorizationService.AuthorizationAsync(Request, Authorization.Email, Authorization.Password);
        if (!string.IsNullOrEmpty(redirectUri))
        {
            return Redirect(redirectUri);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt");

        return Page();
    }

    private async Task RequestValidationAsync()
    {
        var clientIsValid = await _authorizationService.ClientValidationAsync(Request);

        QueryIsValid = clientIsValid;
    }
}
