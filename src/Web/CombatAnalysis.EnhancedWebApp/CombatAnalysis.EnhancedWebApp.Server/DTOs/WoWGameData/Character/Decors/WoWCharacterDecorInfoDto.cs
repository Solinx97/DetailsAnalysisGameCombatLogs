using CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Decors;

public class WoWCharacterDecorInfoDto : WoWAccountCollectionItemInfoDto
{
    public int Quantity { get; set; }
}
