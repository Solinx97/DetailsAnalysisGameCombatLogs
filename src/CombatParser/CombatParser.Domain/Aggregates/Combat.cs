using CombatParser.Domain.Entities;
using CombatParser.Domain.EntityData;
using CombatParser.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatParser.Domain.Aggregates;

public class Combat
{
    public const int DUNGEON_NAME_MAX_LENGTH = 128;

    private readonly List<CombatPlayer> _players = [];
    private readonly List<Unit> _units = [];

    private Combat() { }

    private Combat(string dungeonName, double bossHealthPercentage, long damageDone, long healDone, long damageTaken,
        long resourcesRecovery, bool isWin, DateTimeOffset startDate, DateTimeOffset finishDate, int bossId, int combatLogId)
    {
        DungeonName = dungeonName;
        BossHealthPercentage = bossHealthPercentage;
        DamageDone = damageDone;
        HealDone = healDone;
        DamageTaken = damageTaken;
        ResourcesRecovery = resourcesRecovery;
        IsWin = isWin;
        StartDate = startDate;
        FinishDate = finishDate;
        BossId = bossId;
        CombatLogId = combatLogId;
    }

    public int Id { get; private set; }

    public string DungeonName { get; private set; } = string.Empty;

    public double BossHealthPercentage { get; private set; }

    public long DamageDone { get; private set; }

    public long HealDone { get; private set; }

    public long DamageTaken { get; private set; }

    public long ResourcesRecovery { get; private set; }

    public bool IsWin { get; private set; }

    public DateTimeOffset StartDate { get; private set; }

    public DateTimeOffset FinishDate { get; private set; }

    [NotMapped]
    public string Duration
    {
        get { return (FinishDate - StartDate).ToString(@"hh\:mm\:ss"); }
    }

    public Boss Boss { get; private set; }

    public int BossId { get; private set; }

    public CombatLog CombatLog { get; private set; }

    public int CombatLogId { get; private set; }

    public IReadOnlyCollection<CombatPlayer> CombatPlayers => _players.AsReadOnly();

    public IReadOnlyCollection<Unit> Units => _units.AsReadOnly();

    public static Combat Create(string dungeonName, double bossHealthPercentage, long damageDone, long healDone, long damageTaken,
        long resourcesRecovery, bool isWin, DateTimeOffset startDate, DateTimeOffset finishDate, int bossId, int combatLogId,
        IReadOnlyList<CombatPlayerData> combatPlayers, IReadOnlyList<UnitData> units)
    {
        ArgumentException.ThrowIfNullOrEmpty(dungeonName, nameof(dungeonName));
        ArgumentOutOfRangeException.ThrowIfNegative(bossHealthPercentage, nameof(bossHealthPercentage));
        ArgumentOutOfRangeException.ThrowIfNegative(damageDone, nameof(damageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(healDone, nameof(healDone));
        ArgumentOutOfRangeException.ThrowIfNegative(damageTaken, nameof(damageTaken));

        CombatException.ThrowIfLong(dungeonName);
        CombatException.ThrowIfDateIncorrect(startDate, finishDate);

        var combat = new Combat(dungeonName, bossHealthPercentage, damageDone, healDone, damageTaken,
            resourcesRecovery, isWin, startDate, finishDate, bossId, combatLogId);

        foreach (var player in combatPlayers)
        {
            combat.AddCombatPlayer(player);
        }

        foreach (var unit in units)
        {
            combat.AddUnit(unit);
        }

        return combat;
    }

    private void AddCombatPlayer(CombatPlayerData player)
    {
        var createdPlayer = CombatPlayer.Create(player.AverageItemLevel, player.PlayerId, player.Stats, player.Score, player.UnitGameId);
        _players.Add(createdPlayer);
    }

    private void AddUnit(UnitData unit)
    {
        var createdUnit = Unit.Create(unit.GameId, unit.Name, unit.UnitHash, unit.Type, unit.CreatorGameId, unit.UnitInfo,
            unit.UnitHealths, unit.UnitCasts, unit.UnitPositions, unit.PreAuras, unit.Auras, unit.DamageDones,
            unit.HealDones, unit.ResourceRecoveries);
        _units.Add(createdUnit);
    }
}
