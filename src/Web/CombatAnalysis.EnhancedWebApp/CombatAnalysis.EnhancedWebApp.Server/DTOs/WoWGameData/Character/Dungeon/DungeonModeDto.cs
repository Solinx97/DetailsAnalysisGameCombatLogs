namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonModeDto
{
    public DungeonModeTypeDto Difficulty { get; set; }

    public DungeonModeTypeDto Status { get; set; }

    public DungeonModeProgressDto Progress { get; set; }
}
