using System.Security.Cryptography;

namespace CombatAnalysis.Identity.Security;

public static class JWTSecret
{
    public static string AccessSecretKey { get; private set; }

    public static string RefreshSecretKey { get; private set; }

    public static void GenerateAccessSecretKey()
    {
        using var rng = new RNGCryptoServiceProvider();
        byte[] data = new byte[32];
        rng.GetBytes(data);

        AccessSecretKey = Convert.ToBase64String(data);
    }

    public static void GenerateRefreshSecretKey()
    {
        using var rng = new RNGCryptoServiceProvider();
        byte[] data = new byte[32];
        rng.GetBytes(data);

        RefreshSecretKey = Convert.ToBase64String(data);
    }
}
