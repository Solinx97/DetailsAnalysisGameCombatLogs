﻿using CombatAnalysis.Hubs.Consts;

namespace CombatAnalysis.Hubs.Helpers;

internal static class CreateEnvironmentHelper
{
    public static void UseAppsettings(ConfigurationManager configuration)
    {
        CORS.WebApp = configuration["Cors:WebApp"] ?? string.Empty;

        Cluster.Chat = configuration["Cluster:Chat"] ?? string.Empty;

        Authentication.Issuer = configuration["Authentication:Issuer"] ?? string.Empty;
        Authentication.IssuerSigningKey = Convert.FromBase64String(configuration["Authentication:IssuerSigningKey"] ?? string.Empty);
        Authentication.Authority = configuration["Authentication:Authority"] ?? string.Empty;

        AuthenticationClient.WebClientId = configuration["Authentication:Client:WebClientId"] ?? string.Empty;
        AuthenticationClient.DesktopClientId = configuration["Authentication:Client:DesktopClientId"] ?? string.Empty;
        AuthenticationClient.Scope = configuration["Authentication:Client:Scope"] ?? string.Empty;
    }

    public static void UseEnvVariables()
    {
        CORS.WebApp = Environment.GetEnvironmentVariable("Cors_WebApp") ?? string.Empty;

        Cluster.Chat = Environment.GetEnvironmentVariable("Cluster_Chat") ?? string.Empty;

        Authentication.Issuer = Environment.GetEnvironmentVariable("Authentication_Issuer") ?? string.Empty;
        Authentication.IssuerSigningKey = Convert.FromBase64String(Environment.GetEnvironmentVariable("Authentication_IssuerSigningKey") ?? string.Empty);
        Authentication.Authority = Environment.GetEnvironmentVariable("Authentication_Authority") ?? string.Empty;

        AuthenticationClient.WebClientId = Environment.GetEnvironmentVariable("Authentication_Client_WebClientId") ?? string.Empty;
        AuthenticationClient.DesktopClientId = Environment.GetEnvironmentVariable("Authentication_Client_DesktopClientId") ?? string.Empty;
        AuthenticationClient.Scope = Environment.GetEnvironmentVariable("Authentication_Client_Scope") ?? string.Empty;
    }
}
