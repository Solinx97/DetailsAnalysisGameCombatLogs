namespace CombatAnalysis.UploadingLogsApp.Consts;

internal static class JWTToken
{
    public static int AccessTokenExpInSeconds { get; set; } = 3600;

    public static int RefreshTokenExpnSeconds { get; set; } = 2592000;
}
