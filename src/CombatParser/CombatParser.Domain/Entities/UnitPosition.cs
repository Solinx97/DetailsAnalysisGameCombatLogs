using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitPosition : CombatUnitDataBase, ITime, IUnitRef
{
    public const int OWNER_GAMEID_MAX_LENGTH = 128;

    private UnitPosition() { }

    private UnitPosition(string ownerGameId, double x, double y, TimeSpan time)
    {
        Id = Guid.NewGuid().ToString();
        OwnerGameId = ownerGameId;
        X = x;
        Y = y;
        Time = time;
    }

    public string OwnerGameId { get; private set; } = string.Empty;

    public double X { get; private set; }

    public double Y { get; private set; }

    public TimeSpan Time { get; private set; }

    public static UnitPosition Create(string ownerGameId, double x, double y, TimeSpan time)
    {
        ArgumentException.ThrowIfNullOrEmpty(ownerGameId, nameof(ownerGameId));

        return new UnitPosition(ownerGameId, x, y, time);
    }
}