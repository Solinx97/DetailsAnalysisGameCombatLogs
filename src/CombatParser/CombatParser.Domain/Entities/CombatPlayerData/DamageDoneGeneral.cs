using CombatParser.Domain.Entities.Base;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageDoneGeneral : CombatUnitDataBase
{
    public const int SPELL_MAX_LENGTH = 128;

    private DamageDoneGeneral() { }

    private DamageDoneGeneral(int gameSpellId, string spell, int value, double damagePerSecond, int critNumber, 
        int missNumber, int castNumber, int minValue, int maxValue, double averageValue)
    {
        Id = Guid.NewGuid().ToString();
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
    }

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

    public static DamageDoneGeneral Create(int gameSpellId, string spell, int value, double damagePerSecond, int critNumber,
        int missNumber, int castNumber, int minValue, int maxValue, double averageValue)
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

        return new DamageDoneGeneral(gameSpellId, spell, value, damagePerSecond, critNumber,
            missNumber, castNumber, minValue, maxValue, averageValue);
    }
}
