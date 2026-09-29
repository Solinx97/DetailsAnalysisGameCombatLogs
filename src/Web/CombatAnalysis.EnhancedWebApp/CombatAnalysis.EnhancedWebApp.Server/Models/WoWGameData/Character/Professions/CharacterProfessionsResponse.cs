using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Professions;

public class CharacterProfessionsResponse
{
    [JsonPropertyName("primaries")]
    public CharacterProfessionModel[] Primaries { get; set; }

    [JsonPropertyName("secondaries")]
    public CharacterProfessionModel[] Secondaries { get; set; }
}
