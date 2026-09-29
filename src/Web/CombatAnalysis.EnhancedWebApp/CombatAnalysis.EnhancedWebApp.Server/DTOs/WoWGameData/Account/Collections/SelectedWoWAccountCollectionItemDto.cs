namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

public class SelectedWoWAccountCollectionItemDto : WoWGameDataEntityDto
{
    public string Description { get; set; }

    public WoWGameDataTypeDto Source { get; set; }

    public WoWGameDataTypeDto? Faction { get; set; }
}
