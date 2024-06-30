namespace CombatAnalysis.Identity.Interfaces;

public interface IOAuthCodeFlowService
{
    Task<string> GenerateAuthorizationCodeAsync(string userId, string clientId, string codeChallenge, string codeChallengeMethod);

    bool ValidateCodeChallenge(string codeVerifier, string authorizationCode);

    (string AuthorizationCode, string CustomData) DecryptAuthorizationCode(string encryptedDataWithCustomData, byte[] encryptionKey);
}
