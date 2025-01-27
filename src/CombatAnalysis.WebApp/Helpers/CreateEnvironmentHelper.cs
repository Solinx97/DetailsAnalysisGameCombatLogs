using CombatAnalysis.WebApp.Consts;

namespace CombatAnalysis.WebApp.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        Cluster.CombatParser = configuration["Cluster:CombatParser"] ?? string.Empty;
        Cluster.User = configuration["Cluster:User"] ?? string.Empty;
        Cluster.Chat = configuration["Cluster:Chat"] ?? string.Empty;
        Cluster.Communication = configuration["Cluster:Communication"] ?? string.Empty;
        Cluster.Identity = configuration["Cluster:Identity"] ?? string.Empty;

        Servers.Identity = configuration["Servers:Identity"] ?? string.Empty;

        Authentication.CookieDomain = configuration["Authentication:CookieDomain"] ?? string.Empty;
        Authentication.RedirectUri = configuration["Authentication:RedirectUri"] ?? string.Empty;
        Authentication.IdentityAuthPath = configuration["Authentication:IdentityAuthPath"] ?? string.Empty;
        Authentication.IdentityRegistryPath = configuration["Authentication:IdentityRegistryPath"] ?? string.Empty;
        Authentication.CodeChallengeMethod = configuration["Authentication:CodeChallengeMethod"] ?? string.Empty;

        if (int.TryParse(configuration["Authentication:RefreshTokenExpiresDays"], out var refreshTokenExpiresDays))
        {
            Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
        }

        AuthenticationGrantType.Code = configuration["Authentication:GrantType:Code"] ?? string.Empty;
        AuthenticationGrantType.Authorization = configuration["Authentication:GrantType:Authorization"] ?? string.Empty;
        AuthenticationGrantType.RefreshToken = configuration["Authentication:GrantType:RefreshToken"] ?? string.Empty;

        AuthenticationClient.ClientId = configuration["Authentication:Client:ClientId"] ?? string.Empty;
        AuthenticationClient.Scope = configuration["Authentication:Client:Scope"] ?? string.Empty;
    }

    public static void UseEnvVariables()
    {
        Cluster.CombatParser = Environment.GetEnvironmentVariable("Cluster_CombatParser") ?? string.Empty;
        Cluster.User = Environment.GetEnvironmentVariable("Cluster_User") ?? string.Empty;
        Cluster.Chat = Environment.GetEnvironmentVariable("Cluster_Chat") ?? string.Empty;
        Cluster.Communication = Environment.GetEnvironmentVariable("Cluster_Communication") ?? string.Empty;
        Cluster.Identity = Environment.GetEnvironmentVariable("Cluster_Identity") ?? string.Empty;

        Servers.Identity = Environment.GetEnvironmentVariable("Servers_Identity") ?? string.Empty;

        Authentication.CookieDomain = Environment.GetEnvironmentVariable("Authentication_CookieDomain") ?? string.Empty;
        Authentication.RedirectUri = Environment.GetEnvironmentVariable("Authentication_RedirectUri") ?? string.Empty;
        Authentication.IdentityAuthPath = Environment.GetEnvironmentVariable("Authentication_IdentityAuthPath") ?? string.Empty;
        Authentication.IdentityRegistryPath = Environment.GetEnvironmentVariable("Authentication_IdentityRegistryPath") ?? string.Empty;
        Authentication.CodeChallengeMethod = Environment.GetEnvironmentVariable("Authentication_CodeChallengeMethod") ?? string.Empty;

        if (int.TryParse(Environment.GetEnvironmentVariable("Authentication_RefreshTokenExpiresDays"), out var refreshTokenExpiresDays))
        {
            Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
        }

        AuthenticationGrantType.Code = Environment.GetEnvironmentVariable("Authentication_GrantType_Code") ?? string.Empty;
        AuthenticationGrantType.Authorization = Environment.GetEnvironmentVariable("Authentication_GrantType_Authorization") ?? string.Empty;
        AuthenticationGrantType.RefreshToken = Environment.GetEnvironmentVariable("Authentication_GrantType_RefreshToken") ?? string.Empty;

        AuthenticationClient.ClientId = Environment.GetEnvironmentVariable("Authentication_Client_ClientId") ?? string.Empty;
        AuthenticationClient.Scope = Environment.GetEnvironmentVariable("Authentication_Client_Scope") ?? string.Empty;
    }
}
