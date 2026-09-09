using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitHealth : CombatDataBase, ITime, IUnitRef
{
    public const int GAMEID_MAX_LENGTH = 128;

    private UnitHealth() { }

    private UnitHealth(string creatorGameId, long currentHealth, long maxHealth, TimeSpan time, bool isDead, int combatId)
    {
        Id = Guid.NewGuid().ToString();
        CreatorGameId = creatorGameId;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
        Time = time;
        IsDead = isDead;
        CombatId = combatId;
    }

    public string Id { get; private set; }

    public string CreatorGameId { get; private set; }

    public long CurrentHealth { get; private set; }

    public long MaxHealth { get; private set; }

    public TimeSpan Time { get; private set; }

    public bool IsDead { get; private set; }

    public Combat Combat { get; private set; }

    public static UnitHealth Create(string creatorGameId, long currentHealth, long maxHealth, TimeSpan time, bool isDead, int combatId)
    {
        ArgumentException.ThrowIfNullOrEmpty(creatorGameId, nameof(creatorGameId));
        ArgumentOutOfRangeException.ThrowIfNegative(currentHealth, nameof(currentHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(maxHealth, nameof(maxHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(combatId, nameof(combatId));

        return new UnitHealth(creatorGameId, currentHealth, maxHealth, time, isDead, combatId);
    }
}
