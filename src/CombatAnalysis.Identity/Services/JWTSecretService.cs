using CombatAnalysis.CustomerDAL.Interfaces;
using CombatAnalysis.Identity.Interfaces;
using System.Security.Cryptography;

namespace CombatAnalysis.Identity.Services;

public class JWTSecretService : IJWTSecret
{
    private readonly IAppSecret _secretRepository;

    public JWTSecretService(IAppSecret secretRepository)
    {
        _secretRepository = secretRepository;
    }

    public async Task GenerateSecretKeysAsync()
    {
        var resfreSecret = GenerateSecretKeys();
        var accessSecret = GenerateSecretKeys();

        await UpdateSecretKeysAsync(resfreSecret, accessSecret);
    }

    public async Task<CustomerDAL.Entities.Authentication.Secret> GetSecretAsync()
    {
        var secrets = await _secretRepository.GetAllAsync();
        if (!secrets.Any())
        {
            return null;
        }

        return await _secretRepository.GetByIdAsync(secrets.Last().Id);

    }

    private static string GenerateSecretKeys()
    {
        using var rng = new RNGCryptoServiceProvider();
        byte[] data = new byte[32];
        rng.GetBytes(data);

        return Convert.ToBase64String(data);
    }

    private async Task UpdateSecretKeysAsync(string accessSecretKey, string refreshSecretKey)
    {
        var secrets = await _secretRepository.GetAllAsync();
        if (!secrets.Any())
        {
            await SaveSecretKeysAsync(accessSecretKey, refreshSecretKey);
            return;
        }

        var lastSecret = secrets.Last();
        lastSecret.AccessSecret = accessSecretKey;
        lastSecret.RefreshSecret = refreshSecretKey;

        await _secretRepository.UpdateAsync(lastSecret);
    }

    private async Task SaveSecretKeysAsync(string accessSecretKey, string refreshSecretKey)
    {
        var secret = new CustomerDAL.Entities.Authentication.Secret
        {
            AccessSecret = accessSecretKey,
            RefreshSecret = refreshSecretKey,
        };

        await _secretRepository.CreateAsync(secret);
    }
}
