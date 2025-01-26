namespace CombatAnalysis.WebApp.Consts;

public static class Authentication
{
    public static string CookieDomain { get; set; }

    public static string ClientId { get; set; }

    public static string ClientScope { get; set; }

    public static string RedirectUri { get; set; }

    public static string IdentityServer { get; set; }

    public static string IdentityAuthPath { get; set; }

    public static string IdentityRegistryPath { get; set; }

    public static string CodeChallengeMethod { get; set; }

    public static int RefreshTokenExpiresDays { get; set; }
}