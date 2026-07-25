using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class ResourceRecovery : CombatPlayerDataBase, ICombatTime, IGeneralEntity
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

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public string Creator { get; private set; } = string.Empty;

    public string Target { get; private set; } = string.Empty;

    public CombatPlayer CombatPlayer { get; private set; }
}
