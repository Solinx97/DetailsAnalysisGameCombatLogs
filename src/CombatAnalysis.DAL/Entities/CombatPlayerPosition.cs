namespace CombatAnalysis.DAL.Entities;

public class CombatPlayerPosition
{
    public int Id { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public int CombatPlayerId { get; set; }
}
