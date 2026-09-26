namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonExpansionDto
{
    public WoWGameDataEntityDto Expansion { get; set; }

    public DungeonInstanceDto[] Instances { get; set; }
}
