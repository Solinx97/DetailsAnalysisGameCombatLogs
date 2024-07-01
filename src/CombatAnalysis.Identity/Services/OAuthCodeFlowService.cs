using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.Identity.Security;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CombatAnalysis.Identity.Services;

internal class OAuthCodeFlowService : IOAuthCodeFlowService
{
    private readonly IPkeRepository _pkeRepository;
    private readonly IClientRepository _clientRepository;

    public OAuthCodeFlowService(IPkeRepository pkeRepository, IClientRepository clientRepository)
    {
        _pkeRepository = pkeRepository;
        _clientRepository = clientRepository;
    }

    async Task<string> IOAuthCodeFlowService.GenerateAuthorizationCodeAsync(string userId, string clientId, string codeChallenge, string codeChallengeMethod, string redirectUri)
    {
        var authorizationCode = GenerateAuthorizationCode();

        var encryptedAuthorizationCode = EncryptAuthorizationCode(authorizationCode, userId, Encryption.EnctyptionKey);

        await _pkeRepository.CreateAsync(clientId, encryptedAuthorizationCode, codeChallenge, codeChallengeMethod, redirectUri);

        return encryptedAuthorizationCode;
    }

    async Task<bool> IOAuthCodeFlowService.ValidateCodeChallengeAsync(string clientId, string codeVerifier, string authorizationCode, string redirectUri)
    {
        if (string.IsNullOrEmpty(clientId)
            || string.IsNullOrEmpty(codeVerifier)
            || string.IsNullOrEmpty(authorizationCode)
            || string.IsNullOrEmpty(redirectUri))
        {
            return false;
        }

        var authorizationCodeChallenge = await _pkeRepository.GetByIdAsync(authorizationCode);
        if (authorizationCodeChallenge == null)
        {
            return false;
        }

        if (authorizationCodeChallenge.IsUsed)
        {
            return false;
        }

        var redirectUriIsValid = authorizationCodeChallenge.RedirectUrl == redirectUri;
        if (!redirectUriIsValid)
        {
            return false;
        }

        var clientIdIsValid = authorizationCodeChallenge.ClientId == clientId;
        if (!clientIdIsValid)
        {
            return false;
        }

        var generatedCodeChallenge = GenerateCodeChallenge(codeVerifier);
        var codeChallengeIsValid = authorizationCodeChallenge.CodeChallenge == generatedCodeChallenge;
        if (!codeChallengeIsValid)
        {
            return false;
        }

        await MarkCodeAsUsedAsync(authorizationCodeChallenge);

        return true;
    }

    async Task<bool> IOAuthCodeFlowService.ValidateClientAsync(string clientId, string redirectUri, string clientScope)
    {
        if (string.IsNullOrEmpty(clientId)
            || string.IsNullOrEmpty(clientScope)
            || string.IsNullOrEmpty(redirectUri))
        {
            return false;
        }

        var client = await _clientRepository.GetByIdAsync(clientId);
        if (client == null)
        {
            return false;
        }

        var redirectUriIsValid = client.RedirectUrl == redirectUri;
        if (!redirectUriIsValid)
        {
            return false;
        }

        var scopeIsValid = client.Scope == clientScope;

        return scopeIsValid;
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

    string IOAuthCodeFlowService.GenerateToken(string clientId, string userId)
    {
        var key = new SymmetricSecurityKey(Encryption.EnctyptionKey);
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.Aud, clientId),
        };

        var token = new JwtSecurityToken(
            issuer: "https://localhost:7064",
            audience: clientId,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        return accessToken;
    }

    private static string GenerateAuthorizationCode()
    {
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        var randomBytes = new byte[32]; // 256 bits
        randomNumberGenerator.GetBytes(randomBytes);

        var code = Convert.ToBase64String(randomBytes);
        return code;
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

    private async Task<int> MarkCodeAsUsedAsync(AuthorizationCodeChallenge authorizationCodeChallenge)
    {
        authorizationCodeChallenge.IsUsed = true;

        var affectedRows = await _pkeRepository.MarkCodeAsUsedAsync(authorizationCodeChallenge);

        return affectedRows;
    }
}
