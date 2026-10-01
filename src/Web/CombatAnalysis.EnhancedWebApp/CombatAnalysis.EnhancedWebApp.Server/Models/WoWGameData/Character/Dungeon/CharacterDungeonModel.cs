using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Dungeon;

public class CharacterDungeonModel
{
    [JsonPropertyName("character")]
    public WoWGameDataCharacterModel Character { get; set; }

    [JsonPropertyName("expansions")]
    public DungeonExpansionModel[] Expansions { get; set; }
}
