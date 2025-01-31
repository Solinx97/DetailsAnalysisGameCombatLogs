using CombatAnalysis.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.CombatParser.Entities;

public class CombatPlayerPosition : ICombatPlayerEntity
{
    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatId { get; set; }

    public int CombatPlayerId { get; set; }
}
