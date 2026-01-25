namespace CombatParser.Domain.Entities;

public record CombatPlayerPosition
{
    private CombatPlayerPosition() { }

    public CombatPlayerPosition(int positionX, int positionY, TimeSpan time, int combatPlayerId, int combatId)
    {
        PositionX = positionX;
        PositionY = positionY;
        Time = time;
        CombatPlayerId = combatPlayerId;
        CombatId = combatPlayerId;
    }

    public int Id { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public TimeSpan Time { get; set; }

    public int CombatPlayerId { get; set; }

    public int CombatId { get; set; }
}