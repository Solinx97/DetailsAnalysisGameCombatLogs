namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageDone
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

    public int Id { get; private set; }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public string Creator { get; private set; } = string.Empty;

    public string Target { get; private set; } = string.Empty;

    public bool IsTargetBoss { get; private set; }

    public int DamageType { get; private set; }

    public bool IsPeriodicDamage { get; private set; }

    public bool IsSingleTarget { get; private set; }

    public bool IsPet { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public int CombatPlayerId { get; private set; }

    public void SetCombatPlayerId(int combatPlayerId)
    {
        CombatPlayerId = combatPlayerId;
    }
}
