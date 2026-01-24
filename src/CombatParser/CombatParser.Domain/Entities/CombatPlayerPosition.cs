namespace CombatParser.Domain.Entities;

public class CombatPlayerPosition
{
    public int Id { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public TimeSpan Time { get; set; }

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }

    public Combat Combat { get; set; }

    public int CombatId { get; set; }
}