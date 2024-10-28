namespace CombatAnalysis.Identity.Interfaces;

public interface IResetPasswordService
{
    Task<string> GeneratePasswordResetTokenAsync(string email);

    Task<bool> ResetPasswordAsync(string token, string email, string password);
}
