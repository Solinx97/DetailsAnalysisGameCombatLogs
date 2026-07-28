using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities;

public class UnitCast : CombatDataBase, IUnitRef, ITime
{
    public const int SPELL_MAX_LENGTH = 128;
    public const int GAME_MAX_LENGTH = 128;

    private UnitCast() { }

    private UnitCast(string creatorGameId, int gameSpellId, string spell, TimeSpan startTime, TimeSpan finishTime,
         string? targetGameId, bool isImmediatly, bool isSuccess, int combatId)
    {
        Id = Guid.NewGuid().ToString();
        CreatorGameId = creatorGameId;
        GameSpellId = gameSpellId;
        Spell = spell;
        Time = startTime;
        FinishTime = finishTime;
        TargetGameId = targetGameId;
        IsImmediatly = isImmediatly;
        IsSuccess = isSuccess;
        CombatId = combatId;
    }

    public string Id { get; private set; }

    public string CreatorGameId { get; private set; } = string.Empty;

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public TimeSpan Time { get; private set; }

    public TimeSpan FinishTime { get; private set; }

    public string? TargetGameId { get; private set; }

    public bool IsImmediatly { get; private set; }

    public bool IsSuccess { get; private set; }

    public Combat Combat { get; private set; }

    public static UnitCast Create(string creatorGameId, int gameSpellId, string spell, TimeSpan startTime, TimeSpan finishTime,
         string? targetGameId, bool isImmediatly, bool isSuccess, int combatId)
    {
        ArgumentException.ThrowIfNullOrEmpty(creatorGameId, nameof(creatorGameId));
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));

        return new UnitCast(creatorGameId, gameSpellId, spell, startTime, finishTime, targetGameId, isImmediatly, isSuccess, combatId);
    }
}
