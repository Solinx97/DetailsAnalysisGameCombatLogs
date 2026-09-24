namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDto
{
    public MythicKeystoneCurrentPeriodDto CurrentPeriod { get; set; }

    public MythicKeystoneSeasonDto[] Seasons { get; set; }

    public DungeonCharacterDto Character { get; set; }

    public MythicKeystoneRaitingDto CurrentMythicRating { get; set; }
}
