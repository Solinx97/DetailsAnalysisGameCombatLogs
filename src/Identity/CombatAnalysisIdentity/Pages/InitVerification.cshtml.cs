using CombatAnalysis.Identity.Interfaces;
using CombatAnalysisIdentity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CombatAnalysisIdentity.Pages;

public class InitVerificationModel : PageModel
{
    private readonly IUserVerification _userVerification;

    public InitVerificationModel(IUserVerification userVerification)
    {
        _userVerification = userVerification;
    }

    public async Task<IActionResult> OnGet(string email)
    {
        var token = await _userVerification.GenerateVerifyEmailTokenAsync(email);

        var redirectUri = Request.Query["redirectUri"];
        var verifyLink = $"{Request.Scheme}://{Request.Host}/verification?token={token}&redirectUri={redirectUri}";

        await SendVerifyEmailToEmailAsync(email, verifyLink);

        return Page();
    }

    private static async Task SendVerifyEmailToEmailAsync(string email, string verifyLink)
    {
        const string subject = "Email verification";
        string body = $"<p>Click on <a href=\"{verifyLink}\">Verify</a> for verification your email.</p>";

        await EmailService.SendResetPasswordEmailAsync(email, subject, body);
    }
}
