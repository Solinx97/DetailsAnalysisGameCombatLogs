namespace CombatParser.Domain.Entities.CombatPlayerData;

public record DamageDone
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int CREATOR_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private DamageDone() { }

    public DamageDone(int gameSpellId, string spell, int value, TimeSpan time, string creator,
        string target, bool isTargetBoss, int damageType, bool isPeriodicDamage, bool isSingleTarget,
        bool isPet, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell)); 
        ArgumentException.ThrowIfNullOrEmpty(creator, nameof(creator));
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target)); 
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value)); 
        ArgumentOutOfRangeException.ThrowIfNegative(damageType, nameof(damageType)); 

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        Creator = creator;
        Target = target;
        IsTargetBoss = isTargetBoss;
        DamageType = damageType;
        IsPeriodicDamage = isPeriodicDamage;
        IsSingleTarget = isSingleTarget;
        IsPet = isPet;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; }

    public int GameSpellId { get; }

    public string Spell { get; } = string.Empty;

    public int Value { get; }

    public TimeSpan Time { get; }

    public string Creator { get; } = string.Empty;

    public string Target { get; } = string.Empty;

    public bool IsTargetBoss { get; }

    public int DamageType { get; }

    public bool IsPeriodicDamage { get; }

    public bool IsSingleTarget { get; }

    public bool IsPet { get; }

    public CombatPlayer CombatPlayer { get; }

    public int CombatPlayerId { get; }
}
