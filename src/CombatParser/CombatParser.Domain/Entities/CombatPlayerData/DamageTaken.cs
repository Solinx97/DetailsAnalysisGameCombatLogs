using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageTaken : CombatPlayerDataBase, ICombatPlayerData
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int CREATOR_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private DamageTaken() { }

    public DamageTaken(int gameSpellId, string spell, int value, TimeSpan time, string creator,
        string target, int damageTakenType, int actualValue, bool isPeriodicDamage, int resisted,
        int absorbed, int blocked, int realDamage, int mitigated, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentException.ThrowIfNullOrEmpty(creator, nameof(creator));
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(damageTakenType, nameof(damageTakenType));
        ArgumentOutOfRangeException.ThrowIfNegative(resisted, nameof(resisted));
        ArgumentOutOfRangeException.ThrowIfNegative(absorbed, nameof(absorbed));
        ArgumentOutOfRangeException.ThrowIfNegative(blocked, nameof(blocked));
        ArgumentOutOfRangeException.ThrowIfNegative(mitigated, nameof(mitigated));

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        Creator = creator;
        Target = target;
        DamageTakenType = damageTakenType;
        ActualValue = actualValue;
        IsPeriodicDamage = isPeriodicDamage;
        Resisted = resisted;
        Absorbed = absorbed;
        Blocked = blocked;
        RealDamage = realDamage;
        Mitigated = mitigated;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; private set; }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public string Creator { get; private set; } = string.Empty;

    public string Target { get; private set; } = string.Empty;

    public int DamageTakenType { get; private set; }

    public int ActualValue { get; private set; }

    public bool IsPeriodicDamage { get; private set; }

    public int Resisted { get; private set; }

    public int Absorbed { get; private set; }

    public int Blocked { get; private set; }

    public int RealDamage { get; private set; }

    public int Mitigated { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}
