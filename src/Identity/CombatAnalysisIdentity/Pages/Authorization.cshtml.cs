using CombatAnalysisIdentity.Consts;
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

    public string RegistrationUrl { get; } = Port.Identity;

    public async Task OnGetAsync()
    {
        await RequestValidationAsync();
    }

    public async Task<IActionResult> OnPostAsync(string email, string password)
    {
        await RequestValidationAsync();

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Login data invalidate");

            return Page();
        }

        var redirectUri = await _authorizationService.AuthorizationAsync(Request, email, password);
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
