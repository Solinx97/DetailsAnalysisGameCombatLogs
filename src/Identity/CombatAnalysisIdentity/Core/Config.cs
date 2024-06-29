using IdentityServer4.Models;

namespace CombatAnalysisIdentity.Core;

public class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "My API #1"),
            new ApiScope("api2", "My API #2")
        };

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new Client
            {
                ClientId = "client1",
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RedirectUris = { "https://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                RequireConsent = true,
                AllowedScopes = { "api1" },
                ClientSecrets =
                {
                    new Secret("secret1".Sha512())
                },
            },
            new Client
            {
                ClientId = "client2",
                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                RedirectUris = { "https://localhost:5002/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                RequireConsent = true,
                AllowedScopes = { "api2" },
                ClientSecrets =
                {
                    new Secret("secret2".Sha512())
                },
            }
        };
    }

    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    }

    public static IEnumerable<ApiResource> GetApiResources()
    {
        return new List<ApiResource>
        {
            new ApiResource("api1", "My API 1"),
            new ApiResource("api2", "My API 2"),
        };
    }
}
