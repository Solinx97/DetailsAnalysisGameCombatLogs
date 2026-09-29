using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Professions;

public class CharacterProfessionTierModel
{
    [JsonPropertyName("skill_points")]
    public int SkillPoints { get; set; }

    [JsonPropertyName("max_skill_points")]
    public int MaxSkillPoints { get; set; }

    [JsonPropertyName("tier")]
    public WoWGameDataEntityModel Tier { get; set; }

    [JsonPropertyName("known_recipes")]
    public WoWGameDataEntityModel[] KnownRecipes { get; set; }
}
