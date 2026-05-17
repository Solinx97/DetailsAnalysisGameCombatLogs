using HealthAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace HealthAPI.Extensions;

internal static class JwtBearerOptionsExtension
{
    public static async Task<JsonWebKeySet> GetJWKSAsync(this JwtBearerOptions _, string authority)
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
