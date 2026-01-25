using CombatParser.Domain.Entities;
using CombatParser.Domain.EntityData;
using CombatParser.Domain.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace CombatParser.Domain.Aggregates;

public class Combat
{
    public const int DUNGEON_NAME_MAX_LENGTH = 128;

    private readonly List<CombatPlayer> _players = [];

    private Combat() { }

    private Combat(string dungeonName, double bossHealthPercentage, long damageDone, long healDone, long damageTaken,
        long resourcesRecovery, bool isWin, DateTimeOffset startDate, DateTimeOffset finishDate, int bossId,
        int combatLogId)
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
        IsReady = false;
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

    public bool IsReady { get; private set; }

    public int BossId { get; private set; }

    public CombatLog CombatLog { get; private set; }

    public int CombatLogId { get; private set; }

    public IReadOnlyCollection<CombatPlayer> CombatPlayers => _players.AsReadOnly();

    //public ICollection<CombatPlayerPosition> CombatPlayerPositions { get; set; } = [];

    //public ICollection<CombatAura> CombatAuras { get; set; } = [];

    //public ICollection<CombatTarget> CombatTargets { get; set; } = [];

    public static Combat Create(string dungeonName, double bossHealthPercentage, long damageDone, long healDone, long damageTaken,
        long resourcesRecovery, bool isWin, DateTimeOffset startDate, DateTimeOffset finishDate, int bossId,
        int combatLogId, IReadOnlyList<CombatPlayerData> combatPlayers)
    {
        ArgumentException.ThrowIfNullOrEmpty(dungeonName, nameof(dungeonName));
        ArgumentOutOfRangeException.ThrowIfNegative(bossHealthPercentage, nameof(bossHealthPercentage));
        ArgumentOutOfRangeException.ThrowIfNegative(damageDone, nameof(damageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(healDone, nameof(healDone));
        ArgumentOutOfRangeException.ThrowIfNegative(damageTaken, nameof(damageTaken));
        ArgumentOutOfRangeException.ThrowIfNegative(resourcesRecovery, nameof(resourcesRecovery));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(combatLogId, nameof(combatLogId));

        CombatException.ThrowIfLong(dungeonName);
        CombatException.ThrowIfDateIncorrect(startDate, finishDate);

        var combat = new Combat(dungeonName, bossHealthPercentage, damageDone, healDone, damageTaken, resourcesRecovery, isWin, startDate, finishDate, bossId, combatLogId);

        foreach (var player in combatPlayers)
        {
            combat.AddCombatPlayer(player);
        }

        return combat;
    }

    public void CombatIsReady()
    {
        IsReady = true;
    }

    private void AddCombatPlayer(CombatPlayerData player)
    {
        var createdPlayer = CombatPlayer.Create(player.AverageItemLevel, player.ResourcesRecovery, player.DamageDone, player.HealDone, player.DamageTaken,
            player.PlayerId, player.CombatId, player.Stats, player.Score, player.DamageDones,
            player.DamageDoneGenerals, player.HealDones, player.HealDoneGenerals, player.DamageTakens, player.DamageTakenGenerals,
            player.ResourceRecoveries, player.ResourceRecoveryGenerals, player.CombatPlayerDeaths, player.CombatPlayerPositions);
        _players.Add(createdPlayer);
    }
}
