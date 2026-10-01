namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class CharacterDungeonDto
{
    public WoWGameDataCharacterDto Character { get; set; }

    public DungeonExpansionDto[] Expansions { get; set; }
}
