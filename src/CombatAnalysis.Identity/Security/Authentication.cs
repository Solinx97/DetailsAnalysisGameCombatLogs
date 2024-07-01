using System.Security.Cryptography;

namespace CombatAnalysis.Identity.Security;

public static class Authentication
{
    public static byte[] IssuerSigningKey { get; set; }

    public static string Issuer { get; set; }

    public static int TokenExpiresInMinutes { get; set; }

    public static void GenerateAesKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();

        IssuerSigningKey = aes.Key;
    }
}