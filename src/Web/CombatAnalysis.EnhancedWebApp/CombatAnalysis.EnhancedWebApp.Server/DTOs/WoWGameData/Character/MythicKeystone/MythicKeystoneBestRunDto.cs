namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneBestRunDto
{
    public long CompletedTimestamp { get; set; }

    public int Duration { get; set; }

    public int Level { get; set; }

    public MythicKeystoneAfixDto[] Afixes { get; set; }

    public MythicKeystoneMemberDto[] Members { get; set; }

    public MythicKeystoneDungeonDto Dungeon { get; set; }

    public bool IsCompletedWithinTime { get; set; }

    public MythicKeystoneRaitingDto MythicRating { get; set; }

    public MythicKeystoneRaitingDto MapRating { get; set; }
}
