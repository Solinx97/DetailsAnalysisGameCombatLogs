using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentCraftingStatModel : WoWGameDataEntityModel
{
    [JsonPropertyName("type")]
    public string Type { get; set; }
}
