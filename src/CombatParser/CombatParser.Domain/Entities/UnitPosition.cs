using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitPosition : CombatUnitDataBase, ITime, IUnitRef
{
    public const int GAMEID_MAX_LENGTH = 128;

    private UnitPosition() { }

    private UnitPosition(string gameId, double x, double y, TimeSpan time)
    {
        Id = Guid.NewGuid().ToString();
        CreatorGameId = gameId;
        X = x;
        Y = y;
        Time = time;
    }

    public string CreatorGameId { get; private set; } = string.Empty;

    public double X { get; private set; }

    public double Y { get; private set; }

    public TimeSpan Time { get; private set; }

    public CombatUnit CombatUnit { get; private set; }

    public static UnitPosition Create(string gameId, double x, double y, TimeSpan time)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));

        return new UnitPosition(gameId, x, y, time);
    }
}