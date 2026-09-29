using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWPetsResponse
{
    [JsonPropertyName("pets")]
    public WoWGameDataEntityModel[] Pets { get; set; }
}
