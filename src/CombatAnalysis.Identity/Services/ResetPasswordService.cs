using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using System.Security.Cryptography;

namespace CombatAnalysis.Identity.Services;

internal class ResetPasswordService : IResetPasswordService
{
    private readonly IResetTokenRepository _resetTokenRepository;
    private readonly IIdentityUserService _identityUserService;

    public ResetPasswordService(IResetTokenRepository resetTokenRepository, IIdentityUserService identityUserService)
    {
        _resetTokenRepository = resetTokenRepository;
        _identityUserService = identityUserService;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var token = GenerateToken();

        var resetToken = new ResetToken
        {
            Email = email,
            Token = token,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        await _resetTokenRepository.CreateAsync(resetToken);

        return token;
    }

    public async Task<bool> ResetPasswordAsync(string token, string password)
    {
        var resetToken = await _resetTokenRepository.GetByTokenAsync(token);
        if (resetToken == null || resetToken.IsUsed || resetToken.ExpirationTime < DateTime.UtcNow)
        {
            return false;
        }

        var (hash, salt) = PasswordHashing.HashPasswordWithSalt(password);

        var identityUser = await _identityUserService.GetAsync(resetToken.Email);
        identityUser.PasswordHash = hash;
        identityUser.Salt = salt;

        await _identityUserService.UpdateAsync(identityUser);

        resetToken.IsUsed = true;
        await _resetTokenRepository.UpdateAsync(resetToken);

        return true;
    }

    private static string GenerateToken()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var randomBytes = new byte[32]; // 256 bits
        randomNumberGenerator.GetBytes(randomBytes);

        var code = Convert.ToBase64String(randomBytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .Replace("=", "");

        return code;
    }
}
