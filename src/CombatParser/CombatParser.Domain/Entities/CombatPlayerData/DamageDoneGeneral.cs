using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageDoneGeneral : CombatPlayerDataBase, ICombatPlayerData
{
    public const int SPELL_MAX_LENGTH = 128;

    private DamageDoneGeneral() { }

    public DamageDoneGeneral(int gameSpellId, string spell, int value, double damagePerSecond, int critNumber, 
        int missNumber, int castNumber, int minValue, int maxValue, double averageValue,
        bool isPet, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(damagePerSecond, nameof(damagePerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(critNumber, nameof(critNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(missNumber, nameof(missNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(castNumber, nameof(castNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(minValue, nameof(minValue));
        ArgumentOutOfRangeException.ThrowIfNegative(maxValue, nameof(maxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(averageValue, nameof(averageValue));

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        DamagePerSecond = damagePerSecond;
        CritNumber = critNumber;
        MissNumber = missNumber;
        CastNumber = castNumber;
        MinValue = minValue;
        MaxValue = maxValue;
        AverageValue = averageValue;
        IsPet = isPet;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; private set; }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public double DamagePerSecond { get; private set; }

    public int CritNumber { get; private set; }

    public int MissNumber { get; private set; }

    public int CastNumber { get; private set; }

    public int MinValue { get; private set; }

    public int MaxValue { get; private set; }

    public double AverageValue { get; private set; }

    public bool IsPet { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}
