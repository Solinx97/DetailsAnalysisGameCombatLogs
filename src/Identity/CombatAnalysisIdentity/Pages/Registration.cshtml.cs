using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using CombatAnalysisIdentity.Security;
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

    public string Protocol { get; } = Authentication.Protocol;

    public async Task OnGetAsync()
    {
        await RequestValidationAsync();
    }

    public async Task<IActionResult> OnPostAsync(string email, string password, string confirmPassword, int phoneNumber, DateTimeOffset birthday, string username, string firstName, string lastName, string country, string city, int postCode)
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

        var passwordIsStrong = _authorizationService.IsPasswordStrong(password);
        if (!passwordIsStrong)
        {
            ModelState.AddModelError(string.Empty, "Password should have at least 8 characters, upper/lowercase character, digit and special symbol");

            return Page();
        }

        FillIdentityUser(email, password);
        FillAppUser(phoneNumber, birthday, username, firstName, lastName);
        FillCustomer(country, city, postCode);

        return await CreateUserAsync(password);
    }

    private void FillIdentityUser(string email, string password)
    {
        var (hash, salt) = PasswordHashing.HashPasswordWithSalt(password);
        _identityUser = new IdentityUserModel
        {
            Id = Guid.NewGuid().ToString(),
            Email = email,
            PasswordHash = hash,
            Salt = salt
        };
    }

    private void FillAppUser(int phoneNumber, DateTimeOffset birthday, string username, string firstName, string lastName)
    {
        _appUser = new AppUserModel
        {
            Username = username,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            Birthday = birthday,
            IdentityUserId = _identityUser.Id
        };
    }

    private void FillCustomer(string country, string city, int postCode)
    {
        _customer = new CustomerModel
        {
            Country = country,
            City = city,
            PostalCode = postCode,
            AppUserId = _appUser.Id,
        };
    }

    private async Task<IActionResult> CreateUserAsync(string password)
    {
        var wasCreated = await _authorizationService.CreateUserAsync(_identityUser, _appUser, _customer);
        if (!wasCreated)
        {
            return Page();
        }

        var redirectUri = await _authorizationService.AuthorizationAsync(Request, _identityUser.Email, password);
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
