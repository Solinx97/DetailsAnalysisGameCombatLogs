using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitHealth : CombatDataBase, ICombatTime
{
    public const int GAMEID_MAX_LENGTH = 128;

    private UnitHealth() { }

    private UnitHealth(string gamePlayerId, int currentHealth, int maxHealth, TimeSpan time, bool isDead, int combatId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gamePlayerId, nameof(gamePlayerId));
        ArgumentOutOfRangeException.ThrowIfNegative(currentHealth, nameof(currentHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(maxHealth, nameof(maxHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(combatId, nameof(combatId));

        Id = Guid.NewGuid().ToString();
        GamePlayerId = gamePlayerId;
        CurrentHealth = currentHealth;
        MaxHealth = maxHealth;
        Time = time;
        IsDead = isDead;
        CombatId = combatId;
    }

    public string Id { get; private set; }

    public string GamePlayerId { get; private set; }

    public int CurrentHealth { get; private set; }

    public int MaxHealth { get; private set; }

    public TimeSpan Time { get; private set; }

    public bool IsDead { get; private set; }

    public Combat Combat { get; private set; }

    public static UnitHealth Create(string gamePlayerId, int currentHealth, int maxHealth, TimeSpan time, bool isDead, int combatId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gamePlayerId, nameof(gamePlayerId));
        ArgumentOutOfRangeException.ThrowIfNegative(currentHealth, nameof(currentHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(maxHealth, nameof(maxHealth));
        ArgumentOutOfRangeException.ThrowIfNegative(combatId, nameof(combatId));

        return new UnitHealth(gamePlayerId, currentHealth, maxHealth, time, isDead, combatId);
    }
}
