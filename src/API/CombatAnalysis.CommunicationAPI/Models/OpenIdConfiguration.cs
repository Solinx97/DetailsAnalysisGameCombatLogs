using Newtonsoft.Json;

namespace CombatAnalysis.CommunicationAPI.Models;

public class OpenIdConfiguration
{
    [JsonProperty("jwks_uri")]
    public new string JwksUri { get; set; } = default!;
}
