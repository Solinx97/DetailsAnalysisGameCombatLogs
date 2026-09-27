namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account;

public class WoWAccountDto
{
    public long Id { get; set; }

    public Dictionary<string, CharacterDto[]> Characters { get; set; }
}
