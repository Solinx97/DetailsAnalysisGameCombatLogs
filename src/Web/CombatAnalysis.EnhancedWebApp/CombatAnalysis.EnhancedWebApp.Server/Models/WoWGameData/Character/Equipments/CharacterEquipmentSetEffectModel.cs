using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentSetEffectModel
{
    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }

    [JsonPropertyName("required_count")]
    public int RequiredCount { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }
}
