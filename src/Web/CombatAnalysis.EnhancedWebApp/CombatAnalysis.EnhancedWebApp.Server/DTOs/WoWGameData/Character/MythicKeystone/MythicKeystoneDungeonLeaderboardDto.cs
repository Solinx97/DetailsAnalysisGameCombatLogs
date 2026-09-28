namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardDto
{
    public WoWGameDataEntityDto Map { get; set; }

    public int Period { get; set; }

    public DateTimeOffset PeriodStartTime { get; set; }

    public DateTimeOffset PeriodEndTime { get; set; }

    public MythicKeystoneDungeonLeaderboardGroupDto[] LeadingGroups { get; set; }

    public MythicKeystoneDungeonLeaderboardAfixDto[] Afixes { get; set; }

    public int MapChallengeModeId { get; set; }

    public string Name { get; set; }
}
