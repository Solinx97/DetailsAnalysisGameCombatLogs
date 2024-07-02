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

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync(int step, string email, string password, string confirmPassword, int phoneNumber, DateTimeOffset birthday, string username, string firstName, string lastName, string aboutMe)
    {
        if (!password.Equals(confirmPassword))
        {
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
}
