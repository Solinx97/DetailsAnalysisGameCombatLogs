using CombatAnalysis.WebApp.Consts;

namespace CombatAnalysis.WebApp.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        API.CombatParser = configuration["API:CombatParser"] ?? string.Empty;
        API.User = configuration["API:User"] ?? string.Empty;
        API.Chat = configuration["API:Chat"] ?? string.Empty;
        API.Communication = configuration["API:Communication"] ?? string.Empty;
        API.Identity = configuration["API:Identity"] ?? string.Empty;

        AuthenticationGrantType.Code = configuration["Authentication:GrantType:Code"] ?? string.Empty;
        AuthenticationGrantType.Authorization = configuration["Authentication:GrantType:Authorization"] ?? string.Empty;
        AuthenticationGrantType.RefreshToken = configuration["Authentication:GrantType:RefreshToken"] ?? string.Empty;

        Authentication.CookieDomain = configuration["Authentication:CookieDomain"] ?? string.Empty;
        Authentication.ClientId = configuration["Authentication:ClientId"] ?? string.Empty;
        Authentication.ClientScope = configuration["Authentication:ClientScope"] ?? string.Empty;
        Authentication.RedirectUri = configuration["Authentication:RedirectUri"] ?? string.Empty;
        Authentication.IdentityAuthPath = configuration["Authentication:IdentityAuthPath"] ?? string.Empty;
        Authentication.IdentityRegistryPath = configuration["Authentication:IdentityRegistryPath"] ?? string.Empty;
        Authentication.CodeChallengeMethod = configuration["Authentication:CodeChallengeMethod"] ?? string.Empty;

        if (int.TryParse(configuration["Authentication:RefreshTokenExpiresDays"], out var refreshTokenExpiresDays))
        {
            Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
        }
    }

    public static void UseEnvVariables()
    {
        API.CombatParser = Environment.GetEnvironmentVariable("API_CombatParser") ?? string.Empty;
        API.User = Environment.GetEnvironmentVariable("API_User") ?? string.Empty;
        API.Chat = Environment.GetEnvironmentVariable("API_Chat") ?? string.Empty;
        API.Communication = Environment.GetEnvironmentVariable("API_Communication") ?? string.Empty;
        API.Identity = Environment.GetEnvironmentVariable("API_Identity") ?? string.Empty;

        AuthenticationGrantType.Code = Environment.GetEnvironmentVariable("Authentication_GrantType_Code") ?? string.Empty;
        AuthenticationGrantType.Authorization = Environment.GetEnvironmentVariable("Authentication_GrantType_Authorization") ?? string.Empty;
        AuthenticationGrantType.RefreshToken = Environment.GetEnvironmentVariable("Authentication_GrantType_RefreshToken") ?? string.Empty;

        Authentication.CookieDomain = Environment.GetEnvironmentVariable("Authentication_CookieDomain") ?? string.Empty;
        Authentication.ClientId = Environment.GetEnvironmentVariable("Authentication_ClientId") ?? string.Empty;
        Authentication.ClientScope = Environment.GetEnvironmentVariable("Authentication_ClientScope") ?? string.Empty;
        Authentication.RedirectUri = Environment.GetEnvironmentVariable("Authentication_RedirectUri") ?? string.Empty;
        Authentication.IdentityAuthPath = Environment.GetEnvironmentVariable("Authentication_IdentityAuthPath") ?? string.Empty;
        Authentication.IdentityRegistryPath = Environment.GetEnvironmentVariable("Authentication_IdentityRegistryPath") ?? string.Empty;
        Authentication.CodeChallengeMethod = Environment.GetEnvironmentVariable("Authentication_CodeChallengeMethod") ?? string.Empty;

        if (int.TryParse(Environment.GetEnvironmentVariable("Authentication_RefreshTokenExpiresDays"), out var refreshTokenExpiresDays))
        {
            Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
        }
    }
}
