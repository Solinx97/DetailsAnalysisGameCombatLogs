using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneMemberModel
{
    [JsonPropertyName("character")]
    public DungeonCharacterModel Character { get; set; }

    [JsonPropertyName("specialization")]
    public CharacterSpecializationModel Specialization { get; set; }

    [JsonPropertyName("race")]
    public CharacterRaceModel Race { get; set; }

    [JsonPropertyName("equipped_item_level")]
    public int EquippedItemLevel { get; set; }
}
