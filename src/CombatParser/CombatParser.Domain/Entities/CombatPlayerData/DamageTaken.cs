namespace CombatParser.Domain.Entities.CombatPlayerData;

public record DamageTaken
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

    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public int DamageTakenType { get; set; }

    public int ActualValue { get; set; }

    public bool IsPeriodicDamage { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Mitigated { get; set; }

    public int CombatPlayerId { get; set; }
}
