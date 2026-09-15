using CombatParser.Domain.Entities.Base;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class ResourceRecoveryGeneral : CombatUnitDataBase
{
    public const int SPELL_MAX_LENGTH = 128;

    private ResourceRecoveryGeneral() { }

    private ResourceRecoveryGeneral(int gameSpellId, string spell, int value, double resourcePerSecond, int castNumber,
        int minValue, int maxValue, double averageValue)
    {
        Id = Guid.NewGuid().ToString();
        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        ResourcePerSecond = resourcePerSecond;
        CastNumber = castNumber;
        MinValue = minValue;
        MaxValue = maxValue;
        AverageValue = averageValue;
    }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public double ResourcePerSecond { get; private set; }

    public int CastNumber { get; private set; }

    public int MinValue { get; private set; }

    public int MaxValue { get; private set; }

    public double AverageValue { get; private set; }

    public static ResourceRecoveryGeneral Create(int gameSpellId, string spell, int value, double resourcePerSecond, int castNumber,
        int minValue, int maxValue, double averageValue)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(castNumber, nameof(castNumber));

        return new ResourceRecoveryGeneral(gameSpellId, spell, value, resourcePerSecond, castNumber,
            minValue, maxValue, averageValue);
    }
}
