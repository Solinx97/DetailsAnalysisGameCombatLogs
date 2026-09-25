namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonInstanceDto
{
    public DungeonDto Instance { get; set; }

    public DungeonModeDto[] Modes { get; set; }
}
