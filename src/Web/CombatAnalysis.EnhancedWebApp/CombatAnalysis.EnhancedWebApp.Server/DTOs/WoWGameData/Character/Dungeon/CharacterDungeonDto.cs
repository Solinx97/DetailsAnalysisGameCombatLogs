using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class CharacterDungeonDto
{
    public DungeonCharacterDto Character { get; set; }

    public DungeonExpansionDto[] Expansions { get; set; }
}
