using CombatAnalysis.UserApi.Consts;

namespace CombatAnalysis.UserApi.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        API.Identity = configuration["API:Identity"] ?? string.Empty;

        Authentication.IssuerSigningKey = configuration["Authentication:IssuerSigningKey"] ?? string.Empty;
        Authentication.Authority = configuration["Authentication:Authority"] ?? string.Empty;
        Authentication.Audience = configuration["Authentication:Audience"] ?? string.Empty;

        AuthenticationClient.ClientId = configuration["Authentication:Client:ClientId"] ?? string.Empty;
        AuthenticationClient.ClientSecret = configuration["Authentication:Client:ClientSecret"] ?? string.Empty;
        AuthenticationClient.Scope = configuration["Authentication:Client:Scope"] ?? string.Empty;
    }

    public static void UseEnvVariables()
    {
        API.Identity = Environment.GetEnvironmentVariable("API_Identity") ?? string.Empty;

        Authentication.IssuerSigningKey = Environment.GetEnvironmentVariable("Authentication_IssuerSigningKey") ?? string.Empty;
        Authentication.Authority = Environment.GetEnvironmentVariable("Authentication_Authority") ?? string.Empty;
        Authentication.Audience = Environment.GetEnvironmentVariable("Authentication_Audience") ?? string.Empty;

        AuthenticationClient.ClientId = Environment.GetEnvironmentVariable("Authentication_Client_ClientId") ?? string.Empty;
        AuthenticationClient.ClientSecret = Environment.GetEnvironmentVariable("Authentication_Client_ClientSecret") ?? string.Empty;
        AuthenticationClient.Scope = Environment.GetEnvironmentVariable("Authentication_Client_Scope") ?? string.Empty;
    }
}

