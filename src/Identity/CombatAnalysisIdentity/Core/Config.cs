using Duende.IdentityServer.Models;

namespace CombatAnalysisIdentity.Core;

internal class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("api.read", "Allow read to API"),
            new ApiScope("api.write", "Allow write to API"),
            new ApiScope("offline_access", "Allow refresh_token"),
        ];

    public static IEnumerable<Client> GetClients()
    {
        return
        [
            new Client
            {
                ClientId = "desktop-app",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "http://localhost:45571/callback" },
                PostLogoutRedirectUris = { "http://localhost:45571/" },

                RequirePkce = true,
                RequireClientSecret = false,

                AllowedScopes = { "api.read", "api.write", "offline_access" },
                AllowAccessTokensViaBrowser = true,

                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 2592000
            },
            new Client
            {
                ClientId = "web-app",
                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "http://localhost:5173/callback" },
                PostLogoutRedirectUris = { "http://localhost:5173/" },

                RequirePkce = true,
                RequireClientSecret = false,

                AllowedScopes = { "api.read", "api.write", "offline_access" },
                AllowAccessTokensViaBrowser = true,

                AllowOfflineAccess = true,
                AccessTokenLifetime = 3600,
                RefreshTokenUsage = TokenUsage.OneTimeOnly,
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 2592000
            },
            new Client
            {
                ClientId = "api",
                ClientSecrets = { new Secret("supersecret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api.read", "api.write" },
                AllowOfflineAccess = true,
            },
        ];
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return
        [
            new ApiResource("user-api", "Protected User API")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
            new ApiResource("chat-api", "Protected Chat API")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
            new ApiResource("communication-api", "Protected Communication API")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
            new ApiResource("combat-parser-api", "Protected Combat parser API")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
            new ApiResource("notification-api", "Protected Notification API")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
            new ApiResource("hubs", "Protected Hubs")
            {
                Scopes = { "api.read", "api.write" },
                UserClaims = { "aud" }
            },
        ];
    }
}
