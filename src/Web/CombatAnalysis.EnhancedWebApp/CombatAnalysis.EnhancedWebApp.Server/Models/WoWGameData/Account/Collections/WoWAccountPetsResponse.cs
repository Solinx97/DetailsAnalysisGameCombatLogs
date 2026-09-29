using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountPetsResponse
{
    [JsonPropertyName("pets")]
    public WoWAccountPetModel[] Pets { get; set; }
}
