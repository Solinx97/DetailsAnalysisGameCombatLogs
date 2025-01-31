namespace CombatAnalysis.WebApp.Consts;

internal static class Authentication
{
    public static string CookieDomain { get; set; }

    public static string RedirectUri { get; set; }

    public static string IdentityAuthPath { get; set; }

    public static string IdentityRegistryPath { get; set; }

    public static string CodeChallengeMethod { get; set; }

    public static int RefreshTokenExpiresDays { get; set; }
}