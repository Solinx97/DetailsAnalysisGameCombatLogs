using CombatAnalysis.Identity.Security;
using CombatAnalysisIdentity.Consts;

namespace CombatAnalysisIdentity.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        DatabaseProps.DefaultConnectionString = configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
        DatabaseProps.UserConnectionString = configuration["ConnectionStrings:UserConnection"] ?? string.Empty;

        API.User = configuration["API:User"] ?? string.Empty;
        API.Identity = configuration["API:Identity"] ?? string.Empty;

        Authentication.Issuer = configuration["Authentication:Issuer"] ?? string.Empty;
        Authentication.IssuerSigningKey = Convert.FromBase64String(configuration["Authentication:IssuerSigningKey"] ?? string.Empty);
        Authentication.Protocol = configuration["Authentication:Protocol"] ?? string.Empty;
        if (int.TryParse(configuration["Authentication:AccessTokenExpiresMins"], out var accessTokenExpiresMins))
        {
            Authentication.AccessTokenExpiresMins = accessTokenExpiresMins;
        }

        if (int.TryParse(configuration["Authentication:RefreshTokenExpiresDays"], out var refreshTokenExpiresDays))
        {
            Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
        }

        AuthenticationGrantType.Code = configuration["Authentication:GrantType:Code"] ?? string.Empty;
        AuthenticationGrantType.Authorization = configuration["Authentication:GrantType:Authorization"] ?? string.Empty;
        AuthenticationGrantType.RefreshToken = configuration["Authentication:GrantType:RefreshToken"] ?? string.Empty;

        Certificate.PfxPath = configuration["Certificate:PfxPath"] ?? string.Empty;
        Certificate.PWD = configuration["Certificate:PWD"] ?? string.Empty;

        SmtpSettings.Host = configuration["Smtp:Host"] ?? string.Empty;
        SmtpSettings.DisplayName = configuration["Smtp:DisplayName"] ?? string.Empty;
        SmtpSettings.Email = configuration["Smtp:Email"] ?? string.Empty;
        SmtpSettings.Password = configuration["Smtp:Password"] ?? string.Empty;
        if (int.TryParse(configuration["Smtp:Port"], out var port))
        {
            SmtpSettings.Port = port;
        }

        if (bool.TryParse(configuration["Smtp:EnableSsl"], out var enableSsl))
        {
            SmtpSettings.EnableSsl = enableSsl;
        }

        if (bool.TryParse(configuration["Smtp:UseDefaultCredentials"], out var useDefaultCredentials))
        {
            SmtpSettings.UseDefaultCredentials = useDefaultCredentials;
        }
    }

    public static void UseEnvVariables()
    {
        DatabaseProps.DefaultConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings_DefaultConnection") ?? string.Empty;
        DatabaseProps.UserConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings_UserConnection") ?? string.Empty;

        API.User = Environment.GetEnvironmentVariable("API_User") ?? string.Empty;
        API.Identity = Environment.GetEnvironmentVariable("API_Identity") ?? string.Empty;

        Authentication.Issuer = Environment.GetEnvironmentVariable("Authentication_Issuer") ?? string.Empty;
        Authentication.IssuerSigningKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("Authentication_IssuerSigningKey") ?? string.Empty);
        Authentication.Protocol = Environment.GetEnvironmentVariable("Authentication_Protocol") ?? string.Empty;
        if (int.TryParse(Environment.GetEnvironmentVariable("Authentication_AccessTokenExpiresMins"), out var accessTokenExpiresMins))
        {
            Authentication.AccessTokenExpiresMins = accessTokenExpiresMins;
        }

        if (int.TryParse(Environment.GetEnvironmentVariable("Authentication_RefreshTokenExpiresDays"), out var refreshTokenExpiresDays))
        {
            Authentication.RefreshTokenExpiresDays = refreshTokenExpiresDays;
        }

        AuthenticationGrantType.Code = Environment.GetEnvironmentVariable("Authentication_GrantType_Code") ?? string.Empty;
        AuthenticationGrantType.Authorization = Environment.GetEnvironmentVariable("Authentication_GrantType_Authorization") ?? string.Empty;
        AuthenticationGrantType.RefreshToken = Environment.GetEnvironmentVariable("Authentication_GrantType_RefreshToken") ?? string.Empty;

        Certificate.PfxPath = Environment.GetEnvironmentVariable("Certificate_PfxPath") ?? string.Empty;
        Certificate.PWD = Environment.GetEnvironmentVariable("Certificate_PWD") ?? string.Empty;

        SmtpSettings.Host = Environment.GetEnvironmentVariable("Smtp_Host") ?? string.Empty;
        SmtpSettings.DisplayName = Environment.GetEnvironmentVariable("Smtp_DisplayName") ?? string.Empty;
        SmtpSettings.Email = Environment.GetEnvironmentVariable("Smtp_Email") ?? string.Empty;
        SmtpSettings.Password = Environment.GetEnvironmentVariable("Smtp_Password") ?? string.Empty;
        if (int.TryParse(Environment.GetEnvironmentVariable("Smtp_Port"), out var port))
        {
            SmtpSettings.Port = port;
        }

        if (bool.TryParse(Environment.GetEnvironmentVariable("Smtp_EnableSsl"), out var enableSsl))
        {
            SmtpSettings.EnableSsl = enableSsl;
        }

        if (bool.TryParse(Environment.GetEnvironmentVariable("Smtp_UseDefaultCredentials"), out var useDefaultCredentials))
        {
            SmtpSettings.UseDefaultCredentials = useDefaultCredentials;
        }
    }
}
