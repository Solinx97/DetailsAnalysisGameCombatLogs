using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class RegistrationModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;
    private IdentityUserModel _identityUser;
    private AppUserModel _appUser;
    private CustomerModel _customer;

    public RegistrationModel(IUserAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    public bool QueryIsValid { get; set; }

    public string AuthorizationUrl { get; } = Port.Identity;

    public async Task OnGetAsync()
    {
        await RequestValidationAsync();
    }

    public async Task<IActionResult> OnPostAsync(int step, string email, string password, string confirmPassword, int phoneNumber, DateTimeOffset birthday, string username, string firstName, string lastName, string aboutMe)
    {
        await RequestValidationAsync();

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError(string.Empty, "Registration data invalidate");

            return Page();
        }

        if (!password.Equals(confirmPassword))
        {
            ModelState.AddModelError(string.Empty, "Password and confirm password should be equal");

            return Page();
        }

        var isPresent = await _authorizationService.CheckIfIdentityUserPresentAsync(email);
        if (isPresent)
        {
            ModelState.AddModelError(string.Empty, "User with this Email already present");

            return Page();
        }

        var usernameAlreadyUsed = await _authorizationService.CheckIfUsernameAlreadyUsedAsync(username);
        if (usernameAlreadyUsed)
        {
            ModelState.AddModelError(string.Empty, "Username already used");

            return Page();
        }

        FillIdentityUser(email, password);
        FillAppUser(phoneNumber, birthday, username, firstName, lastName, aboutMe);
        FillCustomer(username, firstName, lastName, aboutMe);

        return await CreateUserAsync();
    }

    private void FillIdentityUser(string email, string password)
    {
        _identityUser = new IdentityUserModel
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            Password = password
        };
    }

    private void FillAppUser(int phoneNumber, DateTimeOffset birthday, string username, string firstName, string lastName, string aboutMe)
    {
        _appUser = new AppUserModel
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Birthday = birthday,
            AboutMe = aboutMe,
            IdentityUserId = _identityUser.Id
        };
    }

    private void FillCustomer(string username, string firstName, string lastName, string aboutMe)
    {
        _customer = new CustomerModel
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            AboutMe = aboutMe,
            AppUserId = _appUser.Id,
            Message = string.Empty
        };
    }

    private async Task<IActionResult> CreateUserAsync()
    {
        var wasCreated = await _authorizationService.CreateUserAsync(_identityUser, _appUser, _customer);
        if (!wasCreated)
        {
            return Page();
        }

        var redirectUri = await _authorizationService.AuthorizationAsync(Request, _identityUser.Email, _identityUser.Password);
        if (!string.IsNullOrEmpty(redirectUri))
        {
            return Redirect(redirectUri);
        }

        return Page();
    }

    private async Task RequestValidationAsync()
    {
        var clientIsValid = await _authorizationService.ClientValidationAsync(Request);

        QueryIsValid = clientIsValid;
    }
}
