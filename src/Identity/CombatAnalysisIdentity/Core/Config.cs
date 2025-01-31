using Duende.IdentityServer.Models;

namespace CombatAnalysisIdentity.Core;

internal class Config
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
                RedirectUris = { "https://localhost:44479/callback" },
                PostLogoutRedirectUris = { "https://localhost:44479" },
                RequirePkce = true,
                AllowPlainTextPkce = false,
                RequireConsent = true,
                AllowedScopes = { "api1" },
                ClientSecrets =
                {
                    new Secret("secret1".Sha512())
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
