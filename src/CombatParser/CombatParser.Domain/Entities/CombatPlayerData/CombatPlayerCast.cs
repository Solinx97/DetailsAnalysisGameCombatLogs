namespace CombatParser.Domain.Entities.CombatPlayerData;

public class CombatPlayerCast : CombatPlayerDataBase
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int CREATOR_MAX_LENGTH = 128;
    public const int TARGET_MAX_LENGTH = 128;

    private CombatPlayerCast() { }

    public CombatPlayerCast(int gameSpellId, string spell, TimeSpan? startTime, TimeSpan finishTime,
        string creator, string target, bool isSuccess, int combatPlayerId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentException.ThrowIfNullOrEmpty(creator, nameof(creator));
        ArgumentException.ThrowIfNullOrEmpty(target, nameof(target));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));

        GameSpellId = gameSpellId;
        Spell = spell;
        StartTime = startTime;
        FinishTime = finishTime;
        Creator = creator;
        Target = target;
        IsSuccess = isSuccess;
        CombatPlayerId = combatPlayerId;
    }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public TimeSpan? StartTime { get; private set; }

    public TimeSpan FinishTime { get; private set; }

    public string Creator { get; private set; } = string.Empty;

    public string Target { get; private set; } = string.Empty;

    public bool IsSuccess { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }
}
