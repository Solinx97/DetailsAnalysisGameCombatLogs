using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;

public class CharacterSpecializationModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}
