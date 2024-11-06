using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;
using CombatAnalysisIdentity.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Mail;

namespace CombatAnalysisIdentity.Pages;

public class RestoreModel : PageModel
{
    private readonly IUserAuthorizationService _authorizationService;
    private readonly IResetPasswordService _resetPasswordService;
    private readonly ILogger<RestoreModel> _logger;

    public RestoreModel(IUserAuthorizationService authorizationService, IResetPasswordService resetPasswordService, ILogger<RestoreModel> logger)
    {
        _authorizationService = authorizationService;
        _resetPasswordService = resetPasswordService;
        _logger = logger;
    }

    public string AppUrl { get; } = Port.Identity;

    public string Protocol { get; } = Authentication.Protocol;

    public int SendEmailRespond { get; private set;  }

    public async Task<IActionResult> OnPostAsync(string email)
    {
        try
        {
            var isPresent = await _authorizationService.CheckIfIdentityUserPresentAsync(email);
            if (!isPresent)
            {
                ModelState.AddModelError(string.Empty, "User with this Email not present");

                return Page();
            }

            var token = await _resetPasswordService.GeneratePasswordResetTokenAsync(email);

            var redirectUri = Request.Query["redirectUri"];
            var resetLink = $"{Request.Scheme}://{Request.Host}/newPassword?token={token}&redirectUri={redirectUri}";

            await SendResetPasswordEmailAsync(email, resetLink);

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

    private static async Task SendResetPasswordEmailAsync(string email, string resetLink)
    {
        var fromAddress = new MailAddress("no-reply@combat.analysis.com", "Combat Analysis");
        var toAddress = new MailAddress(email);
        const string subject = "Password Reset";
        string body = $"<p>Click on <a href=\"{resetLink}\">Reset link</a> to reset your password.</p>";

        var smtp = new SmtpClient
        {
            Host = "localhost",
            Port = 25,
            EnableSsl = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = true,
        };

        using var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        await smtp.SendMailAsync(message);
    }
}
