namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Character.Collections;

public class SelectedMountDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public WoWGameDataTypeDto Source { get; set; }

    public WoWGameDataTypeDto? Faction { get; set; }
}
