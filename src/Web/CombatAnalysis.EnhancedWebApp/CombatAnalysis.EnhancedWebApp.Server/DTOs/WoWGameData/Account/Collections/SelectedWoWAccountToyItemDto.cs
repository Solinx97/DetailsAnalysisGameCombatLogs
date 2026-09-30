namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Account.Collections;

public class SelectedWoWAccountToyItemDto
{
    public int Id { get; set; }

    public WoWGameDataEntityDto Item { get; set; }

    public string Description { get; set; }

    public WoWGameDataTypeDto Source { get; set; }
}
