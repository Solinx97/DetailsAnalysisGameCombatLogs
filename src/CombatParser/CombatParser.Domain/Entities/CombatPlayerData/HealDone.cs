namespace CombatParser.Domain.Entities.CombatPlayerData;

public record HealDone
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

    public int Id { get; }

    public int GameSpellId { get; }

    public string Spell { get; } = string.Empty;

    public int Value { get; }

    public TimeSpan Time { get; }

    public string Creator { get; }

    public string Target { get; }

    public int Overheal { get; }

    public bool IsCrit { get; }

    public bool IsAbsorbed { get; }

    public int CombatPlayerId { get; }
}
