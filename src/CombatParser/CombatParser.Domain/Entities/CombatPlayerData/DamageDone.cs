using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageDone : CombatUnitDataBase, ITime, IGeneralEntity
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int CREATOR_GAME_ID_MAX_LENGTH = 128;

    private DamageDone() { }

    private DamageDone(int gameSpellId, string spell, int value, TimeSpan time, string creatorId,
        string targetId, int modificationType, int damageType, int resisted,
        int absorbed, int blocked, int realDamage, int overkill, int mitigated, string creatorGameId, string targetGameId)
    {
        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        CreatorId = creatorId;
        CreatorGameId = creatorGameId;
        TargetId = targetId;
        TargetGameId = targetGameId;
        ModificationType = modificationType;
        DamageType = damageType;
        Resisted = resisted;
        Absorbed = absorbed;
        Blocked = blocked;
        RealDamage = realDamage;
        Overkill = overkill;
        Mitigated = mitigated;
    }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; }

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public CombatUnit Creator { get; private set; }

    public CombatUnit Target { get; private set; }

    public int ModificationType { get; private set; }

    public int DamageType { get; private set; }

    public int Resisted { get; private set; }

    public int Absorbed { get; private set; }

    public int Blocked { get; private set; }

    public int RealDamage { get; private set; }

    public int Overkill { get; private set; }

    public int Mitigated { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public static DamageDone Create(int gameSpellId, string spell, int value, TimeSpan time, string creatorId,
        string targetId, int modificationType, int damageType, int resisted, 
        int absorbed, int blocked, int realDamage, int overkill, int mitigated, string creatorGameId, string targetGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentException.ThrowIfNullOrEmpty(creatorGameId, nameof(creatorGameId));
        ArgumentException.ThrowIfNullOrEmpty(targetGameId, nameof(targetGameId));

        return new DamageDone(gameSpellId, spell, value, time, creatorId,
            targetId, modificationType, damageType, resisted, absorbed,
            blocked, realDamage, overkill, mitigated, creatorGameId, targetGameId);
    }
}
