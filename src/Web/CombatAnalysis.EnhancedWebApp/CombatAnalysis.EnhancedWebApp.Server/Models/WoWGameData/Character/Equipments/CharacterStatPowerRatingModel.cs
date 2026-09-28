using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterStatPowerRatingModel
{
    [JsonPropertyName("rating_bonus")]
    public double RatingBonus { get; set; }

    [JsonPropertyName("value")]
    public double? Value { get; set; }

    [JsonPropertyName("rating_normalized")]
    public double RatingNormalized { get; set; }
}
