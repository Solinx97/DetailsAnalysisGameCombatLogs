namespace CombatAnalysis.Identity.Interfaces;

public interface IUserVerification
{
    Task<string> GenerateResetTokenAsync(string email);

    Task<string> GenerateVerifyEmailTokenAsync(string email);

    Task<bool> ResetPasswordAsync(string token, string password);

    Task<bool> VerifyEmailAsync(string token);

    Task RemoveExpiredVerificationAsync();
}
