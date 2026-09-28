namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardGroupDto
{
    public int Ranking { get; set; }

    public TimeSpan Duration { get; set; }

    public DateTimeOffset CompletedTime { get; set; }

    public int KeystoneLevel { get; set; }

    public MythicKeystoneDungeonLeaderboardGroupMemberDto[] Members { get; set; }

    public MythicKeystoneRaitingDto MythicRating { get; set; }
}
