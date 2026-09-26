using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentsResponse
{
    [JsonPropertyName("equipped_items")]
    public CharacterEquipmentModel[] EquippedItems { get; set; }

    [JsonPropertyName("equipped_item_sets")]
    public CharacterEquipmentSetModel[] EquippedItemSets { get; set; }
}
