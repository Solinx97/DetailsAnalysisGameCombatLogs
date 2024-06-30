using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.IdentityDAL.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CombatAnalysis.Identity.Services;

internal class OAuthCodeFlowService : IOAuthCodeFlowService
{
    private readonly IPkeRepository _pkeRepository;

    public OAuthCodeFlowService(IPkeRepository pkeRepository)
    {
        _pkeRepository = pkeRepository;
    }

    async Task<string> IOAuthCodeFlowService.GenerateAuthorizationCodeAsync(string userId, string clientId, string codeChallenge, string codeChallengeMethod)
    {
        var authorizationCode = GenerateAuthorizationCode();
        Encryption.EnctyptionKey = GenerateAesKey();

        var encryptedAuthorizationCode = EncryptAuthorizationCode(authorizationCode, userId, Encryption.EnctyptionKey);

        await _pkeRepository.SaveCodeChallengeAsync(clientId, encryptedAuthorizationCode, codeChallenge, codeChallengeMethod);

        return encryptedAuthorizationCode;
    }

    bool IOAuthCodeFlowService.ValidateCodeChallenge(string codeVerifier, string authorizationCode)
    {
        if (string.IsNullOrEmpty(codeVerifier))
        {
            return false;
        }

        var authorizationCodeChallenge = _pkeRepository.GetCodeChallenge(authorizationCode);
        var generatedCodeChallenge = GenerateCodeChallenge(codeVerifier);
        var codeChallengeIsValidated = authorizationCodeChallenge.CodeChallenge == generatedCodeChallenge;

        return codeChallengeIsValidated;
    }

    private static string EncryptAuthorizationCode(string authorizationCode, string customData, byte[] encryptionKey)
    {
        string combinedData = $"{authorizationCode}:{customData}";

        using var aes = Aes.Create();
        aes.Key = encryptionKey;
        aes.GenerateIV();

        var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(combinedData);
        }

        var iv = aes.IV;
        var encrypted = ms.ToArray();

        var result = new byte[iv.Length + encrypted.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(encrypted, 0, result, iv.Length, encrypted.Length);

        var encryptedAuthorizationKey = Convert.ToBase64String(result);

        return encryptedAuthorizationKey;
    }

    (string AuthorizationCode, string CustomData) IOAuthCodeFlowService.DecryptAuthorizationCode(string encryptedDataWithCustomData, byte[] encryptionKey)
    {
        var fullCipher = Convert.FromBase64String(encryptedDataWithCustomData);

        using var aes = Aes.Create();
        aes.Key = encryptionKey;

        var iv = new byte[aes.BlockSize / 8];
        var cipher = new byte[fullCipher.Length - iv.Length];

        Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
        Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var ms = new MemoryStream(cipher);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        var decryptedData = sr.ReadToEnd();
        // Assuming the authorization code and custom data were concatenated with a colon (":")
        var parts = decryptedData.Split(new[] { ':' }, 2);

        var decryptedAuthorizationKey = (parts[0], parts.Length > 1 ? parts[1] : string.Empty);

        return decryptedAuthorizationKey;
    }

    private static string GenerateAuthorizationCode()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var randomBytes = new byte[32]; // 256 bits
        randomNumberGenerator.GetBytes(randomBytes);

        return Convert.ToBase64String(randomBytes);
    }

    private static byte[] GenerateAesKey()
    {
        using var aes = Aes.Create();
        aes.GenerateKey();

        return aes.Key;
    }

    private static string GenerateCodeChallenge(string verifier)
    {
        var challengeBytes = SHA256.HashData(Encoding.UTF8.GetBytes(verifier));

        var codeChallenge = Convert.ToBase64String(challengeBytes)
                      .TrimEnd('=')
                      .Replace('+', '-')
                      .Replace('/', '_');

        return codeChallenge;
    }
}
