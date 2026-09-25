using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;

public class CharacterTitleModel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }
}
