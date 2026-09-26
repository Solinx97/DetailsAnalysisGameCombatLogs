using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentStatModel
{
    [JsonPropertyName("type")]
    public WoWGameDataTypeModel? Type { get; set; }

    [JsonPropertyName("value")]
    public int Value { get; set; }

    [JsonPropertyName("is_negated")]
    public bool? IsNegated { get; set; }

    [JsonPropertyName("display")]
    public WoWGameDataItemDisplayModel Display { get; set; }
}
