using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.Base;
using CombatParser.Domain.Interfaces;

namespace CombatParser.Domain.Entities.CombatPlayerData;

public class ResourceRecovery : CombatPlayerUnitDataBase, ITime, IGeneralEntity
{
    public const int SPELL_MAX_LENGTH = 128;

    private ResourceRecovery() { }

    private ResourceRecovery(int gameSpellId, string spell, int value, TimeSpan time, int modificationType,
        string creatorId, string targetId, string creatorGameId, string targetGameId)
    {
        GameSpellId = gameSpellId;
        Spell = spell;
        Value = value;
        Time = time;
        ModificationType = modificationType;
        CreatorId = creatorId;
        CreatorGameId = creatorGameId;
        TargetId = targetId;
        TargetGameId = targetGameId;
    }

    public int GameSpellId { get; private set; }

    public string Spell { get; private set; } = string.Empty;

    public int Value { get; private set; }

    public TimeSpan Time { get; private set; }

    public int ModificationType { get; private set; }

    public Unit Creator { get; private set; }

    public Unit Target { get; private set; }

    public CombatPlayer CombatPlayer { get; private set; }

    public static ResourceRecovery Create(int gameSpellId, string spell, int value, TimeSpan time, int modificationType,
        string creatorId, string targetId, string creatorGameId, string targetGameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(spell, nameof(spell));
        ArgumentOutOfRangeException.ThrowIfNegative(gameSpellId, nameof(gameSpellId));
        ArgumentException.ThrowIfNullOrEmpty(creatorGameId, nameof(creatorGameId));
        ArgumentException.ThrowIfNullOrEmpty(targetGameId, nameof(targetGameId));

        return new ResourceRecovery(gameSpellId, spell, value, time, modificationType, creatorId, targetId, creatorGameId, targetGameId);
    }
}
