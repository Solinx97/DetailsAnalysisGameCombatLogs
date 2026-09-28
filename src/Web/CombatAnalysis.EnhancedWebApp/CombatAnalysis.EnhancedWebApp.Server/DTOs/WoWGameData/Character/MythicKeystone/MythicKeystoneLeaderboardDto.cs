namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneLeaderboardDto
{
    public Dictionary<string, MythicKeystoneDungeonLeaderboardDto> CurrentLeaderboards { get; set; } = [];
}
