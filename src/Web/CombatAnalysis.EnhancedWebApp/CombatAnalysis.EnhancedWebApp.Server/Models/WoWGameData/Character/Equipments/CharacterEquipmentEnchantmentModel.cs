using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentEnchantmentModel
{
    [JsonPropertyName("enchantment_id")]
    public int Id { get; set; }

    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }

    [JsonPropertyName("enchantment_slot")]
    public CharacterEquipmentSlotModel Slot { get; set; }
}
