namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardDto
{
    public WoWGameDataEntityDto Map { get; set; }

    public int Period { get; set; }

    public long PeriodStartTimestamp { get; set; }

    public long PeriodEndTimestamp { get; set; }

    public MythicKeystoneDungeonLeaderboardGroupDto[] LeadingGroups { get; set; }

    public MythicKeystoneDungeonLeaderboardAfixDto[] Afixes { get; set; }

    public int MapChallengeModeId { get; set; }

    public string Name { get; set; }
}
