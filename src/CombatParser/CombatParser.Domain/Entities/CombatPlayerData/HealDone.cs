using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class HealDone : CombatPlayerUnitDataBase, ITime, IGeneralEntity
{
    public const int SPELL_MAX_LENGTH = 128;

    private HealDone() { }

    private HealDone(int gameSpellId, string spell, int value, TimeSpan time,
        int overheal, int modificationType, string targetGameId)
    {
        Id = Guid.NewGuid().ToString();
        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        TargetGameId = targetGameId;
        Overheal = overheal;
        ModificationType = modificationType;
    }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public Unit Target { get; private set; }

    public int Overheal { get; private set;  }

    public int ModificationType { get; private set; }

    public static HealDone Create(int gameSpellId, string spell, int value, TimeSpan time,
        int overheal, int modificationType, string targetGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentException.ThrowIfNullOrEmpty(targetGameId, nameof(targetGameId));
        ArgumentOutOfRangeException.ThrowIfNegative(overheal, nameof(overheal));

        return new HealDone(gameSpellId, spell, value, time,
            overheal, modificationType, targetGameId);
    }
}
