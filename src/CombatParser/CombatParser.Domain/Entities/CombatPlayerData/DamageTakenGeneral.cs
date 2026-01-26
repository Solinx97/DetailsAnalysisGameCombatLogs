using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class DamageTakenGeneral : CombatPlayerDataBase, ICombatPlayerData
{
    public const int SPELL_MAX_LENGTH = 128;

    private DamageTakenGeneral() { }

    public DamageTakenGeneral(int gameSpellId, string spell, int value, int actualValue, double damageTakenPerSecond, 
        int missNumber, int critNumber, int castNumber, int minValue, int maxValue, 
        double averageValue, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(actualValue, nameof(actualValue));
        ArgumentOutOfRangeException.ThrowIfNegative(damageTakenPerSecond, nameof(damageTakenPerSecond));
        ArgumentOutOfRangeException.ThrowIfNegative(missNumber, nameof(missNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(critNumber, nameof(critNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(castNumber, nameof(castNumber));
        ArgumentOutOfRangeException.ThrowIfNegative(minValue, nameof(minValue));
        ArgumentOutOfRangeException.ThrowIfNegative(maxValue, nameof(maxValue));
        ArgumentOutOfRangeException.ThrowIfNegative(averageValue, nameof(averageValue));

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        ActualValue = actualValue;
        DamageTakenPerSecond = damageTakenPerSecond;
        MissNumber = missNumber;
        CritNumber = critNumber;
        CastNumber = castNumber;
        MinValue = minValue;
        MaxValue = maxValue;
        AverageValue = averageValue;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; private set; }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public int ActualValue { get; private set; }

    public double DamageTakenPerSecond { get; private set; }

    public int CritNumber { get; private set; }

    public int MissNumber { get; private set; }

    public int CastNumber { get; private set; }

    public int MinValue { get; private set; }

    public int MaxValue { get; private set; }

    public double AverageValue { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}
