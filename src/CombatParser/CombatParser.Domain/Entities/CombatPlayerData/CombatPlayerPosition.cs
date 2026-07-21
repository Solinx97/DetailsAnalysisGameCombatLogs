namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerPosition : CombatPlayerDataBase
{
    private CombatPlayerPosition() { }

    public CombatPlayerPosition(int x, int y, TimeSpan time, int combatPlayerId)
    {
        Id = Guid.NewGuid().ToString();
        X = x;
        Y = y;
        Time = time;
        CombatPlayerId = combatPlayerId;
    }

    public string Id { get; set; } = string.Empty;

    public double X { get; private set; }

    public double Y { get; private set; }

    public TimeSpan Time { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}