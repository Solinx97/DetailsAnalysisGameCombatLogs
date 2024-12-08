namespace CombatAnalysis.CombatParser.Entities;

public class CombatPlayerPosition : CombatDataBase
{
    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatId { get; set; }
}
