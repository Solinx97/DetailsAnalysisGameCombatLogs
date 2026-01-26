using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class HealDone : CombatPlayerDataBase, ICombatPlayerData
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int CREATOR_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private HealDone() { }

    public HealDone(int gameSpellId, string spell, int value, TimeSpan time, string creator,
        string target, int overheal, bool isCrit, bool isAbsorbed, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentException.ThrowIfNullOrEmpty(creator, nameof(creator));
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentOutOfRangeException.ThrowIfNegative(value, nameof(value));
        ArgumentOutOfRangeException.ThrowIfNegative(overheal, nameof(overheal));

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        Creator = creator;
        Target = target;
        Overheal = overheal;
        IsCrit = isCrit;
        IsAbsorbed = isAbsorbed;
        CombatPlayerId = combatPlayerId;
    }

    public int Id { get; private set; }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public string Creator { get; private set; }

    public string Target { get; private set; }

    public int Overheal { get; private set;  }

    public bool IsCrit { get; private set; }

    public bool IsAbsorbed { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}
