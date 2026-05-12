using Newtonsoft.Json;

namespace CombatAnalysis.NotificationAPI.Models;

public class OpenIdConfiguration
{
    [JsonProperty("jwks_uri")]
    public new string JwksUri { get; set; } = default!;
}
