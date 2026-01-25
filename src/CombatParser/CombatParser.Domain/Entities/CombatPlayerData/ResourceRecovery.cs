namespace CombatParser.Domain.Entities.CombatPlayerData;

public record ResourceRecovery
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int CREATOR_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private ResourceRecovery() { }

    public ResourceRecovery(int gameSpellId, string spell, int value, TimeSpan time, string creator,
        string target, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentException.ThrowIfNullOrEmpty(creator, nameof(creator));
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));

        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        Creator = creator;
        Target = target;
        CombatPlayerId = combatPlayerId;
    }


    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string Creator { get; set; } = string.Empty;

    public string Target { get; set; } = string.Empty;

    public int CombatPlayerId { get; set; }
}
