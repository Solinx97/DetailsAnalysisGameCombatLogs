using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using CombatAnalysisIdentity.Models;
using CombatAnalysisIdentity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;

namespace CombatAnalysisIdentity.Pages;

public class RestoreModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;
    private readonly IUserVerification _userVerification;
    private readonly ILogger<RestoreModel> _logger;

    public RestoreModel(IUserAuthorizationService authorizationService, IUserVerification userVerification, ILogger<RestoreModel> logger)
    {
        _authorizationService = authorizationService;
        _userVerification = userVerification;
        _logger = logger;
    }

    public string AppUrl { get; } = API.Identity;

    public string Protocol { get; } = Authentication.Protocol;

    public int SendEmailRespond { get; private set; }

    [BindProperty]
    public RestoreDataModel Restore { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            var isPresent = await _authorizationService.CheckIfIdentityUserPresentAsync(Restore.Email);
            if (!isPresent)
            {
                ModelState.AddModelError(string.Empty, "User with this Email not present");

                return Page();
            }

            var token = await _userVerification.GenerateResetTokenAsync(Restore.Email);

            var redirectUri = Request.Query["redirectUri"];
            var resetLink = $"{Request.Scheme}://{Request.Host}/newPassword?token={token}&redirectUri={redirectUri}";

            await SendResetPasswordToEmailAsync(Restore.Email, resetLink);

            SendEmailRespond = 1;

            return Page();
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "Error sending email");

            ModelState.AddModelError(string.Empty, "Error sending email. Please, try one more time later");

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return Page();
        }
    }

    private static async Task SendResetPasswordToEmailAsync(string email, string resetLink)
    {
        const string subject = "Password Reset";
        string body = $"<p>Click on <a href=\"{resetLink}\">Restore link</a> to reset your password.</p>";

        await EmailService.SendResetPasswordEmailAsync(email, subject, body);
    }
}
