namespace CombatAnalysis.Identity.Interfaces;

public interface IOAuthCodeFlowService
{
    Task<string> GenerateAuthorizationCodeAsync(string userId, string clientId, string codeChallenge, string codeChallengeMethod);

    Task<bool> ValidateCodeChallengeAsync(string codeVerifier, string authorizationCode);

    Task<bool> ValidateClientAsync(string clientId, string redirectUri, string clientScope);

    (string AuthorizationCode, string CustomData) DecryptAuthorizationCode(string encryptedDataWithCustomData, byte[] encryptionKey);

    string GenerateAccessToken(string clientId, string userId);
}
