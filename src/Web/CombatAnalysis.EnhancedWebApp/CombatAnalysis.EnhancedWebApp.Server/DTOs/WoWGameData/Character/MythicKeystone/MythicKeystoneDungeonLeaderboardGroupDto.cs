namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardGroupDto
{
    public int Ranking { get; set; }

    public long Duration { get; set; }

    public long CompletedTimestamp { get; set; }

    public int KeystoneLevel { get; set; }

    public MythicKeystoneDungeonLeaderboardGroupMemberDto[] Members { get; set; }

    public MythicKeystoneRaitingDto MythicRating { get; set; }
}
