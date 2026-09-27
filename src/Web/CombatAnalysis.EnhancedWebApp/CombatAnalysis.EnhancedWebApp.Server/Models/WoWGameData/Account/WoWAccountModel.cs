using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account;

public class WoWAccountModel
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("characters")]
    public CharacterModel[] Characters { get; set; }
}
