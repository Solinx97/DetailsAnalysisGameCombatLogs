using CombatParser.Domain.Entities.Base;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class HealDoneGeneral : CombatUnitDataBase
{
    public const int SPELL_MAX_LENGTH = 128;

    private HealDoneGeneral() { }

    private HealDoneGeneral(int gameSpellId, string spell, int value, double healPerSecond, int critNumber,
        int castNumber, int minValue, int maxValue, double averageValue)
    {
        Id = Guid.NewGuid().ToString();
        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        HealPerSecond = healPerSecond;
        CritNumber = critNumber;
        CastNumber = castNumber;
        MinValue = minValue;
        MaxValue = maxValue;
        AverageValue = averageValue;
    }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public double HealPerSecond { get; private set; }

    public int CritNumber { get; private set; }

    public int CastNumber { get; private set; }

    public int MinValue { get; private set; }

    public int MaxValue { get; private set; }

    public double AverageValue { get; private set; }

    public static HealDoneGeneral Create(int gameSpellId, string spell, int value, double healPerSecond, int critNumber,
        int castNumber, int minValue, int maxValue, double averageValue)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(healPerSecond, nameof(healPerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(critNumber, nameof(critNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(castNumber, nameof(castNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(minValue, nameof(minValue));
        ArgumentOutOfRangeException.ThrowIfNegative(maxValue, nameof(maxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(averageValue, nameof(averageValue));

        return new HealDoneGeneral(gameSpellId, spell, value, healPerSecond, critNumber, 
            castNumber, minValue, maxValue, averageValue);
    }
}
