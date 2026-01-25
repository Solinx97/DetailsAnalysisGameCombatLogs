namespace CombatParser.Domain.Entities.CombatPlayerData;

public record DamageDoneGeneral
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

    public int Id { get; }

    public int GameSpellId { get; }

    public string Spell { get; } = string.Empty;

    public int Value { get; }

    public double DamagePerSecond { get; }

    public int CritNumber { get; }

    public int MissNumber { get; }

    public int CastNumber { get; }

    public int MinValue { get; }

    public int MaxValue { get; }

    public double AverageValue { get; }

    public bool IsPet { get; }

    public int CombatPlayerId { get; }
}
