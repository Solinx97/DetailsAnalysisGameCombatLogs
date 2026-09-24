namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneCurrentPeriodDto
{
    public MythicKeystoneSeasonDto Period { get; set; }

    public MythicKeystoneBestRunDto[] BestRuns { get; set; }
}
