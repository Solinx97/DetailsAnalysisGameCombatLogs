using System.Net.Mail;

namespace CombatAnalysisIdentity.Services;

internal static class EmailService
{
    public static async Task SendResetPasswordEmailAsync(string email, string subject, string body, bool isBodyHtml = true)
    {
        var fromAddress = new MailAddress("no-reply@combat.analysis.com", "Combat Analysis");
        var toAddress = new MailAddress(email);

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
            IsBodyHtml = isBodyHtml,
        };

        await smtp.SendMailAsync(message);
    }
}
