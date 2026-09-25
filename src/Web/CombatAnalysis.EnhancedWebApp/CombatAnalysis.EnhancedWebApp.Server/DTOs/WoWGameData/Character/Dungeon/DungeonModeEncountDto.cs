namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Dungeon;

public class DungeonModeEncountDto
{
    public DungeonDto Encounter { get; set; }

    public int CompletedCount { get; set; }

    public DateTimeOffset LastKillTime { get; set; }
}
