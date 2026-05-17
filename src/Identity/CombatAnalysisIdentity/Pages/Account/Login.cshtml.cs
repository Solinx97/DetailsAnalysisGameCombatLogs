using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace CombatAnalysisIdentity.Pages.Account;

public class LoginModel(IUserAuthorizationService authorizationService, IIdentityServerInteractionService interaction) : PageModel
{
    private readonly IUserAuthorizationService _authorizationService = authorizationService;
    private readonly IIdentityServerInteractionService _interaction = interaction;

    public string CancelRequestAddress { get; private set; } = "cancel=true";

    public string CancelRequestUri { get; private set; } = string.Empty;

    [BindProperty]
    public AuthorizationDataModel? Authorization { get; set; }

    public IActionResult OnGet()
    {
        var nestedParams = HttpUtility.ParseQueryString(new Uri("http://dummy" + Request.Query["ReturnUrl"]).Query);

        string cancelUri = HttpUtility.UrlDecode(nestedParams["cancel_uri"]!);

        CancelRequestUri = cancelUri;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
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

        var success = await _authorizationService.AuthorizationAsync(HttpContext, Authorization.Email, Authorization.Password);
        if (!success)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password");
            return Page();
        }

        Request.Query.TryGetValue("ReturnUrl", out var returnUrlValue);

        var returnUrl = returnUrlValue.ToString();

        var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
        if (context == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid authorization context");
            return Page();
        }

        if (context.Client.ClientId == Clients.Web || context.Client.ClientId == Clients.Desktop)
        {
            return Redirect(returnUrl);
        }

        return Page();
    }
}
