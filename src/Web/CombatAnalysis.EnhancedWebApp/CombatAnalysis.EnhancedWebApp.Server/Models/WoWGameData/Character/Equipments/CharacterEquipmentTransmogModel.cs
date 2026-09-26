using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentTransmogModel
{
    [JsonPropertyName("item")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }

    [JsonPropertyName("item_modified_appearance_id")]
    public int ItemModifiedAppearanceId { get; set; }
}
