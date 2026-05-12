using CombatAnalysis.Hubs.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace CombatAnalysis.Hubs.Helpers;

internal static class JwtBearerOptionsHelper
{
    public static async Task<JsonWebKeySet> GetJWKSAsync(string authority)
    {
        var httpClient = new HttpClient();

        var metadataJson = await httpClient.GetStringAsync(
            $"{authority}.well-known/openid-configuration");

        var metadata = JsonConvert.DeserializeObject<OpenIdConfiguration>(metadataJson);

        var jwksJson = await httpClient.GetStringAsync(metadata!.JwksUri);

        var keySet = new JsonWebKeySet(jwksJson);

        return keySet;
    }
}
