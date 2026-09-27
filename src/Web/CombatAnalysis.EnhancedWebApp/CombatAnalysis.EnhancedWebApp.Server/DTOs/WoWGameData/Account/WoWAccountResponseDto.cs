namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;

public class WoWAccountResponseDto
{
    public long Id { get; set; }

    public WoWAccountDto[] WowAccounts { get; set; }
}
