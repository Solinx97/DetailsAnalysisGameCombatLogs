using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class ResourceRecoveryGeneral : CombatPlayerDataBase, ICombatPlayerRefs
{
    public const int SPELL_MAX_LENGTH = 128;

    private ResourceRecoveryGeneral() { }

    public ResourceRecoveryGeneral(int gameSpellId, string spell, int value, double resourcePerSecond, int castNumber,
        int minValue, int maxValue, double averageValue, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(castNumber, nameof(castNumber));

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

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public double ResourcePerSecond { get; private set; }

    public int CastNumber { get; private set; }

    public int MinValue { get; private set; }

    public int MaxValue { get; private set; }

    public double AverageValue { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}
