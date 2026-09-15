using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitCast : CombatUnitDataBase, ITime, IUnitRef
{
    public const int OWNER_GAME_MAX_LENGTH = 128;
    public const int GAME_SPELL_MAX_LENGTH = 128;
    public const int SPELL_MAX_LENGTH = 128;

    private UnitCast() { }

    private UnitCast(string ownerGameId, int gameSpellId, string spell, TimeSpan startTime, TimeSpan finishTime,
         string? targetGameId, bool isImmediatly, bool isSuccess)
    {
        Id = Guid.NewGuid().ToString();
        OwnerGameId = ownerGameId;
        GameSpellId = gameSpellId;
        Spell = spell;
        Time = startTime;
        FinishTime = finishTime;
        TargetGameId = targetGameId;
        IsImmediatly = isImmediatly;
        IsSuccess = isSuccess;
    }

    public string OwnerGameId { get; private set; } = string.Empty;

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public TimeSpan Time { get; private set; }

    public TimeSpan FinishTime { get; private set; }

    public string? TargetGameId { get; private set; }

    public bool IsImmediatly { get; private set; }

    public bool IsSuccess { get; private set; }

    public Unit Unit { get; private set; }

    public static UnitCast Create(string ownerGameId, int gameSpellId, string spell, TimeSpan startTime, TimeSpan finishTime,
         string? targetGameId, bool isImmediatly, bool isSuccess)
    {
        ArgumentException.ThrowIfNullOrEmpty(ownerGameId, nameof(ownerGameId));
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));

        return new UnitCast(ownerGameId, gameSpellId, spell, startTime, finishTime, targetGameId, isImmediatly, isSuccess);
    }
}
