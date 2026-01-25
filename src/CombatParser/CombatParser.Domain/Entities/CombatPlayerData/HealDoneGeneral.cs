namespace CombatParser.Domain.Entities.CombatPlayerData;

public record HealDoneGeneral
{
    public const int SPELL_MAX_LENGTH = 128;

    private HealDoneGeneral() { }

    public HealDoneGeneral(int gameSpellId, string spell, int value, double healPerSecond, int critNumber,
        int castNumber, int minValue, int maxValue, double averageValue, int combatPlayerId)
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

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        HealPerSecond = healPerSecond;
        CritNumber = critNumber;
        CastNumber = castNumber;
        MinValue = minValue;
        MaxValue = maxValue;
        AverageValue = averageValue;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; }

    public int GameSpellId { get; }

    public string Spell { get; } = string.Empty;

    public int Value { get; }

    public double HealPerSecond { get; }

    public int CritNumber { get; }

    public int CastNumber { get; }

    public int MinValue { get; }

    public int MaxValue { get; }

    public double AverageValue { get; }

    public int CombatPlayerId { get; }
}
