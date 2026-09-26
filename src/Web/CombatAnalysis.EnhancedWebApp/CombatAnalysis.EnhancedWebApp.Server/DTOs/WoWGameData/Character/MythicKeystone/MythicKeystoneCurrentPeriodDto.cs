namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneCurrentPeriodDto
{
    public WoWGameDataEntityDto Period { get; set; }

    public MythicKeystoneBestRunDto[] BestRuns { get; set; }
}
