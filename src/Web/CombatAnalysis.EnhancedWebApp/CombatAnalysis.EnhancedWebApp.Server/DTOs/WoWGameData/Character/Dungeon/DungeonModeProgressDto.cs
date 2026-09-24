namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonModeProgressDto
{
    public int CompletedCount { get; set; }

    public int TotalCount { get; set; }

    public DungeonModeEncountDto[] Encounters { get; set; }
}
