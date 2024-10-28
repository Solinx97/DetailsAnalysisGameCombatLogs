using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace CombatAnalysis.Identity.Security;

public static class PasswordHashing
{
    public static (string hash, string salt) HashPasswordWithSalt(string password)
    {
        byte[] saltBytes = new byte[128 / 8];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(saltBytes);

        var salt = Convert.ToBase64String(saltBytes);

        var hashBytes = KeyDerivation.Pbkdf2(
            password: password,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        var hash = Convert.ToBase64String(hashBytes);

        return (hash, salt);
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);

        var hash = KeyDerivation.Pbkdf2(
            password: enteredPassword,
            salt: saltBytes,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8);

        var passwordIsValid = Convert.ToBase64String(hash) == storedHash;

        return passwordIsValid;
    }
}
