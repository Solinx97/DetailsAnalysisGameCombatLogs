namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonInstanceDto
{
    public WoWGameDataEntityDto Instance { get; set; }

    public DungeonModeDto[] Modes { get; set; }
}
