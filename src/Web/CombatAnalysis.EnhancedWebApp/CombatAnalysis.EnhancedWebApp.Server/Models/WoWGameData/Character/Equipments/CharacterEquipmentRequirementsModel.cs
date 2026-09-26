using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterEquipmentRequirementsModel
{
    [JsonPropertyName("level")]
    public WoWGameDataValueModel Level { get; set; }

    [JsonPropertyName("playable_classes")]
    public WoWGameDataPlayableClassModel PlayableClasses { get; set; }
}
