using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentSpellModel
{
    [JsonPropertyName("spell")]
    public WoWGameDataEntityModel Spell { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("display_color")]
    public WoWGameDataColorModel Color { get; set; }
}
