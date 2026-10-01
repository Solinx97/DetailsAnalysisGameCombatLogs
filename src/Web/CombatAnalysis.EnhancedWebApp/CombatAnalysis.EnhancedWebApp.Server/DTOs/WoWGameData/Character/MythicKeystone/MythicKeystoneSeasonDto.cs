namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneSeasonDto
{
    public MythicKeystoneBestRunDto[] BestRuns { get; set; }

    public WoWGameDataEntityDto Season { get; set; }

    public MythicKeystoneRaitingDto MythicRating { get; set; }
}
