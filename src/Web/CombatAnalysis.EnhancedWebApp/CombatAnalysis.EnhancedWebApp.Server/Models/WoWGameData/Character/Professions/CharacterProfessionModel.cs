using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Professions;

public class CharacterProfessionModel
{
    [JsonPropertyName("profession")]
    public WoWGameDataEntityModel Profession { get; set; }

    [JsonPropertyName("tiers")]
    public CharacterProfessionTierModel[] Tiers { get; set; }
}
