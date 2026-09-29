using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountPetStatModel
{
    [JsonPropertyName("breed_id")]
    public int BreedId { get; set; }

    [JsonPropertyName("health")]
    public int Health { get; set; }

    [JsonPropertyName("power")]
    public int Power { get; set; }

    [JsonPropertyName("speed")]
    public int Speed { get; set; }
}
