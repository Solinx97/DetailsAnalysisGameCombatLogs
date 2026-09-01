namespace CombatAnalysis.WoW_12_1_0.CombatParser.Entities;

public class UnitPosition
{
    public string CreatorGameId { get; set; } = string.Empty;

    public double X { get; set; }

    public double Y { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatId { get; set; }
}
