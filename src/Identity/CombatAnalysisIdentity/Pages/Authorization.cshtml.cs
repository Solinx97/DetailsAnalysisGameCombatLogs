using CombatAnalysis.Identity.Interfaces;
using CombatAnalysisIdentity.Models;
using CombatAnalysisIdentity.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages
{
    public class AuthorizationModel : PageModel
    {
        private readonly IOAuthCodeFlowService _oAuthCodeFlowService;
        private readonly IIdentityUserService _identityUserService;
        private AuthorizationRequestModel _authorizationRequest = new AuthorizationRequestModel();

        public AuthorizationModel(IOAuthCodeFlowService oAuthCodeFlowService, IIdentityUserService identityUserService)
        {
            _oAuthCodeFlowService = oAuthCodeFlowService;
            _identityUserService = identityUserService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl, string email, string password)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return Page();
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                return Page();
            }

            var user = await _identityUserService.GetAsync(email, password);
            if (user != null)
            {
                GetAuthorizationRequestData();

                var authorizationCode = await _oAuthCodeFlowService.GenerateAuthorizationCodeAsync(user.Id, _authorizationRequest.ClientTd, _authorizationRequest.CodeChallenge, _authorizationRequest.CodeChallengeMethod);

                var encodedAuthorizationCode = Uri.EscapeDataString(authorizationCode);
                var redirectUrl = $"{returnUrl}?code={encodedAuthorizationCode}&state=authorized";

                return Redirect(redirectUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return Page();
        }

        private void GetAuthorizationRequestData()
        {
            if (Request.Query.TryGetValue(AuthorizationRequest.RedirectUri.ToString(), out var redirectUri))
            {
                _authorizationRequest.RedirectUri = redirectUri;
            }

            if (Request.Query.TryGetValue(AuthorizationRequest.GrantType.ToString(), out var grantType))
            {
                _authorizationRequest.GrantType = grantType;
            }

            if (Request.Query.TryGetValue(AuthorizationRequest.ClientTd.ToString(), out var clientTd))
            {
                _authorizationRequest.ClientTd = clientTd;
            }

            if (Request.Query.TryGetValue(AuthorizationRequest.Scope.ToString(), out var scope))
            {
                _authorizationRequest.Scope = scope;
            }

            if (Request.Query.TryGetValue(AuthorizationRequest.CodeChallengeMethod.ToString(), out var codeChallengeMethod))
            {
                _authorizationRequest.CodeChallengeMethod = codeChallengeMethod;
            }

            if (Request.Query.TryGetValue(AuthorizationRequest.CodeChallenge.ToString(), out var codeChallenge))
            {
                _authorizationRequest.CodeChallenge = codeChallenge;
            }
        }
    }
}
