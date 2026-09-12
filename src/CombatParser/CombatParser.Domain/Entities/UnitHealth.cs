using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitHealth : CombatUnitDataBase, ITime, IUnitRef
{
    public const int GAMEID_MAX_LENGTH = 128;

    private UnitHealth() { }

    private UnitHealth(string creatorGameId, long currentHealth, long maxHealth, TimeSpan time, bool isDead)
    {
        Id = Guid.NewGuid().ToString();
        CreatorGameId = creatorGameId;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
        Time = time;
        IsDead = isDead;
    }

    public string CreatorGameId { get; private set; }

    public long CurrentHealth { get; private set; }

    public long MaxHealth { get; private set; }

    public TimeSpan Time { get; private set; }

    public bool IsDead { get; private set; }

    public CombatUnit CombatUnit { get; private set; }

    public static UnitHealth Create(string creatorGameId, long currentHealth, long maxHealth, TimeSpan time, bool isDead)
    {
        ArgumentException.ThrowIfNullOrEmpty(creatorGameId, nameof(creatorGameId));
        ArgumentOutOfRangeException.ThrowIfNegative(currentHealth, nameof(currentHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(maxHealth, nameof(maxHealth));

        return new UnitHealth(creatorGameId, currentHealth, maxHealth, time, isDead);
    }
}
