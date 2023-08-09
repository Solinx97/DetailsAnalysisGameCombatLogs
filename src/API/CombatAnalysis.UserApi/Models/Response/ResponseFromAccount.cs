namespace CombatAnalysis.UserApi.Models.Response;

public readonly struct ResponseFromAccount
{
    public ResponseFromAccount(AppUserModel user, string refreshToken)
    {
        User = user;
        RefreshToken = refreshToken;
    }

    public AppUserModel User { get; }

    public string RefreshToken { get; }
}
