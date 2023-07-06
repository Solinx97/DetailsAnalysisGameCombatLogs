namespace CombatAnalysis.UserApi.Models.Response;

public readonly struct ResponseFromAccount
{
    public ResponseFromAccount(AppUserModel user, string accessToke, string refreshToken)
    {
        User = user;
        AccessToken = accessToke;
        RefreshToken = refreshToken;
    }

    public AppUserModel User { get; }

    public string AccessToken { get; }

    public string RefreshToken { get; }
}
