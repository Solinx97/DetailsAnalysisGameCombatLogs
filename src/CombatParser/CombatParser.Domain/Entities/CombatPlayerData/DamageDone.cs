using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageDone : CombatPlayerDataBase, ITime, IGeneralEntity, IDamageRefs
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int CREATOR_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private DamageDone() { }

    private DamageDone(int gameSpellId, string spell, int value, TimeSpan time, string creatorGameId,
        string targetGameId, string targetHash, long targetCurrentHealth, int modificationType, int damageType, int resisted, int absorbed,
        int blocked, int realDamage, int mitigated, int combatPlayerId)
    {
        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        CreatorGameId = creatorGameId;
        TargetGameId = targetGameId;
        TargetHash = targetHash;
        TargetCurrentHealth = targetCurrentHealth;
        ModificationType = modificationType;
        DamageType = damageType;
        Resisted = resisted;
        Absorbed = absorbed;
        Blocked = blocked;
        RealDamage = realDamage;
        Mitigated = mitigated;
        CombatPlayerId = combatPlayerId;
    }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; }

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public string CreatorGameId { get; private set; }

    public string? Creator { get; private set; }

    public string TargetGameId { get; private set; }

    public string? Target { get; private set; }

    public string TargetHash { get; private set; }

    public long TargetCurrentHealth { get; private set; }

    public int ModificationType { get; private set; }

    public int DamageType { get; private set; }

    public int Resisted { get; private set; }

    public int Absorbed { get; private set; }

    public int Blocked { get; private set; }

    public int RealDamage { get; private set; }

    public int Mitigated { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public static DamageDone Create(int gameSpellId, string spell, int value, TimeSpan time, string creatorGameId,
        string targetGameId, string targetHash, long targetCurrentHealth, int modificationType, int damageType, int resisted, 
        int absorbed, int blocked, int realDamage, int mitigated, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentException.ThrowIfNullOrEmpty(creatorGameId, nameof(creatorGameId));
        ArgumentException.ThrowIfNullOrEmpty(targetHash, nameof(targetHash));
        ArgumentException.ThrowIfNullOrEmpty(targetGameId, nameof(targetGameId));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(damageType, nameof(damageType));

        return new DamageDone(gameSpellId, spell, value, time, creatorGameId,
            targetGameId, targetHash, targetCurrentHealth, modificationType, damageType, resisted, absorbed,
            blocked, realDamage, mitigated, combatPlayerId);
    }
}
