using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities;

public class CombatPlayer
{
    public double AverageItemLevel { get; set; }

    public IPlayerStats Stats { get; set; }

    public Player Player { get; set; } = new();

    public string UnitGameId { get; set; } = string.Empty;
}
