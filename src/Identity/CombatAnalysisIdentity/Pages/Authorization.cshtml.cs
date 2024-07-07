using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class AuthorizationModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;

    private AuthorizationRequestModel _authorizationRequest = new AuthorizationRequestModel();

    public AuthorizationModel(IUserAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public bool QueryIsValid { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var clientIsValid = await _authorizationService.ClientValidationAsync(Request);

        QueryIsValid = clientIsValid;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string email, string password)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return Page();
        }

        var redirectUri = await _authorizationService.AuthorizationAsync(Request, email, password);
        if (!string.IsNullOrEmpty(redirectUri))
        {
            return Redirect(redirectUri);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");

        return Page();
    }
}
