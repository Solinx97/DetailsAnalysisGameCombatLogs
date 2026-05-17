using Newtonsoft.Json;

namespace HealthAPI.Models;

public class OpenIdConfiguration
{
    [JsonProperty("jwks_uri")]
    public new string JwksUri { get; set; } = default!;
}
