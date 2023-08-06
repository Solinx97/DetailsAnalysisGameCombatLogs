namespace CombatAnalysis.Identity.Settings;

public static class TokenExpires
{
    public static int AccessExpiresTimeInMinutes { get; } = 15;

    public static int RefreshExpiresTimeInMinutes { get; } = 60;
}
