using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentSetItemModel
{
    [JsonPropertyName("item")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("is_equipped")]
    public bool IsEquipped { get; set; }
}
