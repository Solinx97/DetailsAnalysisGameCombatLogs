using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character;

public class CharacterTitleModel
{
    [JsonPropertyName("name")]
    public GameDataValueNameModel Name { get; set; }

    [JsonPropertyName("display_string")]
    public GameDataValueNameModel DisplayString { get; set; }
}
