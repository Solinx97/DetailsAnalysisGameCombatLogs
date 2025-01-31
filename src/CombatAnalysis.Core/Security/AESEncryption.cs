using System.Security.Cryptography;

namespace CombatAnalysis.Core.Security;

internal static class AESEncryption
{
    private static readonly byte[] Key = Convert.FromBase64String(SecurityKeys.AESKey ?? string.Empty);
    private static readonly byte[] IV = Convert.FromBase64String(SecurityKeys.IV ?? string.Empty);

    public static byte[] EncryptStringToBytes(string plainText)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = IV;

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msEncrypt = new();
        using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
        {
            using StreamWriter swEncrypt = new(csEncrypt);
            swEncrypt.Write(plainText);
        }

        return msEncrypt.ToArray();
    }

    public static string DecryptStringFromBytes(byte[] cipherText)
    {
        using Aes aesAlg = Aes.Create();
        aesAlg.Key = Key;
        aesAlg.IV = IV;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msDecrypt = new(cipherText);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        return srDecrypt.ReadToEnd();
    }
}
