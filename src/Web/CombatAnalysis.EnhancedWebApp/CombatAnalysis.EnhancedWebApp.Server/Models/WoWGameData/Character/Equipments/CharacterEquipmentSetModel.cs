using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentSetModel
{
    [JsonPropertyName("item_set")]
    public WoWGameDataEntityModel ItemSet { get; set; }

    [JsonPropertyName("items")]
    public CharacterEquipmentSetItemModel[] Items { get; set; }

    [JsonPropertyName("effects")]
    public CharacterEquipmentSetEffectModel[] Effects { get; set; }

    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }
}
