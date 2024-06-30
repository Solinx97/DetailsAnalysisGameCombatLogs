using CombatAnalysis.Identity.Interfaces;
using CombatAnalysisIdentity.Security;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace CombatAnalysisIdentity.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IIdentityTokenService _tokenService;

    public TokenController(IIdentityTokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAccessToken(string grantType, string clientId, string codeVerifier, string code, string redirectUri)
    {
        try
        {
            if (string.IsNullOrEmpty(codeVerifier) || string.IsNullOrEmpty(code) || string.IsNullOrEmpty(redirectUri))
            {
                return BadRequest();
            }

            var (authorizationCode, userId) = DecryptAuthorizationCodeWithCustomData(code, Encryption.EnctyptionKey);

            var refreshToken = await _tokenService.GenerateTokensAsync(userId);

            return Ok(refreshToken);
        }
        catch (Exception ex)
        {
            var message = ex.Message;
            return BadRequest();
        }
    }

    private static (string AuthorizationCode, string CustomData) DecryptAuthorizationCodeWithCustomData(string encryptedDataWithCustomData, byte[] encryptionKey)
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
}
