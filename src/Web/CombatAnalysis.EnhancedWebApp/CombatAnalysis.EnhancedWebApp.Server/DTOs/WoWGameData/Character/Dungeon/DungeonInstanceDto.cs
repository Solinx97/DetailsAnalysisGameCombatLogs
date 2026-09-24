namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonInstanceDto
{
    public DungeonNameDto Instance { get; set; }

    public DungeonModeDto[] Modes { get; set; }
}
