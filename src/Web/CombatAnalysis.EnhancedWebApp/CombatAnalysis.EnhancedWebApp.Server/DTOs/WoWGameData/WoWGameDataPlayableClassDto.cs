using CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData;

public class WoWGameDataPlayableClassDto
{
    public WoWGameDataEntityModel[] Links { get; set; }

    public string DisplayString { get; set; }
}
