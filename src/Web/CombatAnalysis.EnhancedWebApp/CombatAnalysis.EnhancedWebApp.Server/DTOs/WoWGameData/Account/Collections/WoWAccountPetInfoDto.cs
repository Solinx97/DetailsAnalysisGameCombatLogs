namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

public class WoWAccountPetInfoDto : WoWAccountCollectionItemInfoDto
{
    public long Id { get; set; }

    public int Level { get; set; }

    public WoWGameDataTypeDto Quality { get; set; }

    public WoWAccountPetStatDto Stats { get; set; }
}
