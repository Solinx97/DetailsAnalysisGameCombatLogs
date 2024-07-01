using System.Security.Cryptography;

namespace CombatAnalysis.Identity.Security;

public static class Encryption
{
    public static byte[] EnctyptionKey { get; set; }

    public static void GenerateAesKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();

        EnctyptionKey = aes.Key;
    }
}