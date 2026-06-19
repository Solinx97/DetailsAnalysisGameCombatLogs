namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerPosition
{
    private CombatPlayerPosition() { }

    public CombatPlayerPosition(int positionX, int positionY, TimeSpan time, int combatPlayerId)
    {
        Id = Guid.NewGuid().ToString();
        PositionX = positionX;
        PositionY = positionY;
        Time = time;
        CombatPlayerId = combatPlayerId;
    }

    public string Id { get; set; } = string.Empty;

    public double PositionX { get; private set; }

    public double PositionY { get; private set; }

    public TimeSpan Time { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public int CombatPlayerId { get; set; }

    public void SetCombatPlayerId(int combatPlayerId)
    {
        CombatPlayerId = combatPlayerId;
    }
}