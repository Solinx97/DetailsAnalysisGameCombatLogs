namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonExpansionDto
{
    public DungeonDto Expansion { get; set; }

    public DungeonInstanceDto[] Instances { get; set; }
}
