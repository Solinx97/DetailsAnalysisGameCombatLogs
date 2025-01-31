using CombatAnalysisIdentity.Consts;
using System.Net;
using System.Net.Mail;

namespace CombatAnalysisIdentity.Services;

internal static class EmailService
{
    public static async Task SendResetPasswordEmailAsync(string email, string subject, string body, bool isBodyHtml = true)
    {
        var fromAddress = new MailAddress(SmtpSettings.Email, SmtpSettings.DisplayName);
        var toAddress = new MailAddress(email);

        var smtp = new SmtpClient
        {
            Host = SmtpSettings.Host,
            Port = SmtpSettings.Port,
            EnableSsl = SmtpSettings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = SmtpSettings.UseDefaultCredentials,
            Credentials = new NetworkCredential(SmtpSettings.Email, SmtpSettings.Password)
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
