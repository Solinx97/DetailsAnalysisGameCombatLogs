namespace CombatParser.Domain.Entities.CombatPlayerData;

public record ResourceRecoveryGeneral
{
    public const int SPELL_MAX_LENGTH = 128;

    private ResourceRecoveryGeneral() { }

    public ResourceRecoveryGeneral(int gameSpellId, string spell, int value, double resourcePerSecond, int castNumber,
        int minValue, int maxValue, double averageValue, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(resourcePerSecond, nameof(resourcePerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(castNumber, nameof(castNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(maxValue, nameof(maxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(averageValue, nameof(averageValue));

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        ResourcePerSecond = resourcePerSecond;
        CastNumber = castNumber;
        MinValue = minValue;
        MaxValue = maxValue;
        AverageValue = averageValue;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public double ResourcePerSecond { get; set; }

    public int CastNumber { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

    public double AverageValue { get; set; }

    public int CombatPlayerId { get; set; }
}
