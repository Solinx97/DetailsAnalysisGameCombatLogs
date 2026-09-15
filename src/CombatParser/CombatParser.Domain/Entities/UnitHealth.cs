using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitHealth : CombatUnitDataBase, ITime, IUnitRef
{
    public const int OWNER_GAMEID_MAX_LENGTH = 128;

    private UnitHealth() { }

    private UnitHealth(string creatorGameId, long currentHealth, long maxHealth, TimeSpan time)
    {
        Id = Guid.NewGuid().ToString();
        OwnerGameId = creatorGameId;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
        Time = time;
    }

    public string OwnerGameId { get; private set; }

    public long CurrentHealth { get; private set; }

    public long MaxHealth { get; private set; }

    public TimeSpan Time { get; private set; }

    public static UnitHealth Create(string ownerGameId, long currentHealth, long maxHealth, TimeSpan time)
    {
        ArgumentException.ThrowIfNullOrEmpty(ownerGameId, nameof(ownerGameId));
        ArgumentOutOfRangeException.ThrowIfNegative(currentHealth, nameof(currentHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(maxHealth, nameof(maxHealth));

        return new UnitHealth(ownerGameId, currentHealth, maxHealth, time);
    }
}
