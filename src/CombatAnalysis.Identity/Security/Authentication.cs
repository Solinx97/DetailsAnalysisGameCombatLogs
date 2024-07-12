namespace CombatAnalysis.Identity.Security;

public static class Authentication
{
    public static byte[] IssuerSigningKey { get; set; }

    public static string Issuer { get; set; }

    public static int AccessTokenExpiresMins { get; set; }

    public static int RefreshTokenExpiresDays { get; set; }

    public static string Protocol { get; set; }
}