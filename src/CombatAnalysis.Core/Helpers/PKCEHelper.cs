using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace CombatAnalysis.Core.Helpers;

internal static class PKCEHelper
{
    public static string GenerateCodeVerifier()
    {
        const int size = 32;
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[size];
        rng.GetBytes(bytes);

        var codeVerifier = WebEncoders.Base64UrlEncode(bytes);
        return codeVerifier;
    }

    public static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));

        var codeChallenge = WebEncoders.Base64UrlEncode(challengeBytes);
        return codeChallenge;
    }
}
