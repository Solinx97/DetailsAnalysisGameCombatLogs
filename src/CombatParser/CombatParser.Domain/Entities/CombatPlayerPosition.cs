namespace CombatParser.Domain.Entities;

public class CombatPlayerPosition
{
    private CombatPlayerPosition() { }

    public CombatPlayerPosition(int positionX, int positionY, TimeSpan time, int combatPlayerId)
    {
        PositionX = positionX;
        PositionY = positionY;
        Time = time;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; private set; }

    public double PositionX { get; private set; }

    public double PositionY { get; private set; }

    public TimeSpan Time { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public int CombatPlayerId { get; private set; }

    public void SetCombatPlayerId(int combatPlayerId)
    {
        CombatPlayerId = combatPlayerId;
    }
}