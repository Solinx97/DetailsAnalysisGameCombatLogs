using CombatAnalysis.CustomerBL.DTO;
using CombatAnalysis.CustomerBL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserService<AppUserDto> _service;

        public LoginModel(IUserService<AppUserDto> service)
        {
            _service = service;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl, string username, string password)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return Page();
            }


            var user = await _service.GetAsync(username, password);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    var authorizationCode = GenerateAuthorizationCode();
                    var redirectUrl = $"{returnUrl}?code={authorizationCode}&state=authorized";

                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return Page();
        }

        private string GenerateAuthorizationCode()
        {
            // Implement your logic here to generate a secure, high-entropy authorization code
            // This is just a placeholder implementation

            return Guid.NewGuid().ToString("N");
        }
    }
}
