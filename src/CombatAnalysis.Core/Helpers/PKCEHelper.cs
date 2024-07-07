using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace CombatAnalysis.Core.Helpers;

internal static class PKCEHelper
{
    public static string GenerateCodeVerifier()
    {
        const int size = 32;
        using (var rng = RandomNumberGenerator.Create())
        {
            var bytes = new byte[size];
            rng.GetBytes(bytes);
            return WebEncoders.Base64UrlEncode(bytes);
        }
    }

    public static async Task<string> GenerateCodeChallengeAsync(string codeVerifier)
    {
        using (var sha256 = SHA256.Create())
        {
            var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
            return WebEncoders.Base64UrlEncode(challengeBytes);
        }
    }
}
