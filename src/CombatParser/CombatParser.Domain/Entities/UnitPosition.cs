using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Entities;

public class UnitPosition : CombatDataBase
{
    public const int GAMEID_MAX_LENGTH = 128;

    private UnitPosition() { }

    private UnitPosition(string gameId, double x, double y, TimeSpan time, int combatId)
    {
        Id = Guid.NewGuid().ToString();
        GameId = gameId;
        X = x;
        Y = y;
        Time = time;
        CombatId = combatId;
    }

    public string Id { get; private set; } = string.Empty;

    public string GameId { get; private set; } = string.Empty;

    public double X { get; private set; }

    public double Y { get; private set; }

    public TimeSpan Time { get; private set; }

    public Combat Combat { get; private set; }

    public static UnitPosition Create(string gameId, double x, double y, TimeSpan time, int combatId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentOutOfRangeException.ThrowIfNegative(combatId, nameof(combatId));

        return new UnitPosition(gameId, x, y, time, combatId);
    }
}