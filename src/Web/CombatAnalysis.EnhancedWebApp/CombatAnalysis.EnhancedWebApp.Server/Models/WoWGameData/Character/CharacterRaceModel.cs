using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;

public class CharacterRaceModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
