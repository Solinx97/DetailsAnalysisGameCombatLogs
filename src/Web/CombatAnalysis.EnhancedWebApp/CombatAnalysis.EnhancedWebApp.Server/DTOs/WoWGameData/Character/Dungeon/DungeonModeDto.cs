namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonModeDto
{
    public WoWGameDataTypeDto Difficulty { get; set; }

    public WoWGameDataTypeDto Status { get; set; }

    public DungeonModeProgressDto Progress { get; set; }
}
