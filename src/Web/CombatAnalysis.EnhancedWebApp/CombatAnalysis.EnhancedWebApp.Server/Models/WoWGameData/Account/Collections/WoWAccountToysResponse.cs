using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountToysResponse
{
    [JsonPropertyName("toys")]
    public WoWAccountToyModel[] Toys { get; set; }
}
