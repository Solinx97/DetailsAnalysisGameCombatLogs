using CombatAnalysis.CommunicationAPI.Consts;

namespace CombatAnalysis.CommunicationAPI.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        DatabaseProps.Name = configuration["Database:Name"] ?? string.Empty;
        DatabaseProps.DataProcessingType = configuration["Database:DataProcessingType"] ?? string.Empty;
        DatabaseProps.MSSQLConnectionString = configuration["ConnectionStrings:DefaultConnection"] ?? string.Empty;
        DatabaseProps.FirebaseConnectionString = configuration["ConnectionStrings:Firebase"] ?? string.Empty;

        API.Identity = configuration["API:Identity"] ?? string.Empty;

        Authentication.Issuer = configuration["Authentication:Issuer"] ?? string.Empty;
        Authentication.IssuerSigningKey = Convert.FromBase64String(configuration["Authentication:IssuerSigningKey"] ?? string.Empty);
        Authentication.Authority = configuration["Authentication:Authority"] ?? string.Empty;

        AuthenticationClient.WebClientId = configuration["Authentication:Client:WebClientId"] ?? string.Empty;
        AuthenticationClient.DesktopClientId = configuration["Authentication:Client:DesktopClientId"] ?? string.Empty;
        AuthenticationClient.Scope = configuration["Authentication:Client:Scope"] ?? string.Empty;
    }

    public static void UseEnvVariables()
    {
        DatabaseProps.Name = Environment.GetEnvironmentVariable("Database_Name") ?? string.Empty;
        DatabaseProps.DataProcessingType = Environment.GetEnvironmentVariable("Database_DataProcessingType") ?? string.Empty;
        DatabaseProps.MSSQLConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings_DefaultConnection") ?? string.Empty;
        DatabaseProps.FirebaseConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings_Firebase") ?? string.Empty;

        API.Identity = Environment.GetEnvironmentVariable("API_Identity") ?? string.Empty;

        Authentication.Issuer = Environment.GetEnvironmentVariable("Authentication_Issuer") ?? string.Empty;
        Authentication.IssuerSigningKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("Authentication_IssuerSigningKey") ?? string.Empty);
        Authentication.Authority = Environment.GetEnvironmentVariable("Authentication_Authority") ?? string.Empty;

        AuthenticationClient.WebClientId = Environment.GetEnvironmentVariable("Authentication_Client_WebClientId") ?? string.Empty;
        AuthenticationClient.DesktopClientId = Environment.GetEnvironmentVariable("Authentication_Client_DesktopClientId") ?? string.Empty;
        AuthenticationClient.Scope = Environment.GetEnvironmentVariable("Authentication_Client_Scope") ?? string.Empty;
    }
}
