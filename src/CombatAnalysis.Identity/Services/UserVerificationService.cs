using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using System.Security.Cryptography;
using System.Transactions;

namespace CombatAnalysis.Identity.Services;

internal class UserVerificationService : IUserVerification
{
    private readonly IResetTokenRepository _resetTokenRepository;
    private readonly IVerifyEmailTokenRepository _verifyEmailRepository;
    private readonly IIdentityUserService _identityUserService;

    public UserVerificationService(IResetTokenRepository resetTokenRepository, IVerifyEmailTokenRepository verifyEmailRepository, IIdentityUserService identityUserService)
    {
        _resetTokenRepository = resetTokenRepository;
        _verifyEmailRepository = verifyEmailRepository;
        _identityUserService = identityUserService;
    }

    public async Task<string> GenerateResetTokenAsync(string email)
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

    public async Task<string> GenerateVerifyEmailTokenAsync(string email)
    {
        var token = GenerateToken();

        var verifyEmailToken = new VerifyEmailToken
        {
            Email = email,
            Token = token,
            ExpirationTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        await _verifyEmailRepository.CreateAsync(verifyEmailToken);

        return token;
    }

    public async Task<bool> ResetPasswordAsync(string token, string password)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var resetToken = await _resetTokenRepository.GetByTokenAsync(token);
        if (resetToken == null || resetToken.IsUsed || resetToken.ExpirationTime < DateTime.UtcNow)
        {
            return false;
        }

        var (hash, salt) = PasswordHashing.HashPasswordWithSalt(password);

        var identityUser = await _identityUserService.GetByEmailAsync(resetToken.Email);
        identityUser.PasswordHash = hash;
        identityUser.Salt = salt;

        await _identityUserService.UpdateAsync(identityUser);

        resetToken.IsUsed = true;
        await _resetTokenRepository.UpdateAsync(resetToken);

        scope.Complete();

        return true;
    }

    public async Task<bool> VerifyEmailAsync(string token)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var verifyToken = await _verifyEmailRepository.GetByTokenAsync(token);
        if (verifyToken == null || verifyToken.IsUsed || verifyToken.ExpirationTime < DateTime.UtcNow)
        {
            return false;
        }

        verifyToken.IsUsed = true;
        await _verifyEmailRepository.UpdateAsync(verifyToken);

        var identityUser = await _identityUserService.GetByEmailAsync(verifyToken.Email);
        if (identityUser == null)
        {
            return false;
        }

        identityUser.EmailVerified = true;
        await _identityUserService.UpdateAsync(identityUser);

        scope.Complete();

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
