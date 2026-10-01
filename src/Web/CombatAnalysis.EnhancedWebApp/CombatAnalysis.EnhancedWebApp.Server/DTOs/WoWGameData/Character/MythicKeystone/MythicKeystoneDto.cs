namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDto
{
    public MythicKeystoneCurrentPeriodDto CurrentPeriod { get; set; }

    public WoWGameDataEntityDto[] Seasons { get; set; }

    public WoWGameDataCharacterDto Character { get; set; }

    public MythicKeystoneRaitingDto CurrentMythicRating { get; set; }
}
