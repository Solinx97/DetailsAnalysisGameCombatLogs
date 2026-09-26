using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentSocketModel
{
    [JsonPropertyName("socket_type")]
    public WoWGameDataTypeModel Type { get; set; }

    [JsonPropertyName("item")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("display_string")]
    public string DisplayString { get; set; }
}
