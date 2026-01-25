using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.EntityData;

namespace CombatParser.Domain.Entities;

public class CombatPlayer
{
    private readonly List<DamageDone> _damageDones = [];
    private readonly List<DamageDoneGeneral> _damageDoneGenerals = [];
    private readonly List<HealDone> _healDones = [];
    private readonly List<HealDoneGeneral> _healDoneGenerals = [];
    private readonly List<DamageTaken> _damageTakens = [];
    private readonly List<DamageTakenGeneral> _damageTakenGenerals = [];
    private readonly List<ResourceRecovery> _resourceRecoveries = [];
    private readonly List<ResourceRecoveryGeneral> _resourceRecoveryGenerals = [];
    private readonly List<CombatPlayerDeath> _combatPlayerDeathes = [];
    private readonly List<CombatPlayerPosition> _combatPlayerPositions = [];

    private CombatPlayer() { }

    private CombatPlayer(double averageItemLevel, int resourcesRecovery, int damageDone, int healDone, int damageTaken, 
        string playerId, int combatId)
    {
        AverageItemLevel = averageItemLevel;
        ResourcesRecovery = resourcesRecovery;
        DamageDone = damageDone;
        HealDone = healDone;
        DamageTaken = damageTaken;
        PlayerId = playerId;
        CombatId = combatId;
    }

    public int Id { get; private set; }

    public double AverageItemLevel { get; private set; }

    public int ResourcesRecovery { get; private set; }

    public int DamageDone { get; private set; }

    public int HealDone { get; private set; }

    public int DamageTaken { get; private set; }

    public CombatPlayerStats Stats { get; private set; }

    public SpecializationScore? Score { get; private set; }

    public Player Player { get; private set; }

    public string PlayerId { get; private set; } = string.Empty;

    public int CombatId { get; private set; }

    public IReadOnlyCollection<DamageDone> DamageDones => _damageDones.AsReadOnly();

    public IReadOnlyCollection<DamageDoneGeneral> DamageDoneGenerals => _damageDoneGenerals.AsReadOnly();

    public IReadOnlyCollection<HealDone> HealDones => _healDones.AsReadOnly();

    public IReadOnlyCollection<HealDoneGeneral> HealDoneGenerals => _healDoneGenerals.AsReadOnly();

    public IReadOnlyCollection<DamageTaken> DamageTakens => _damageTakens.AsReadOnly();

    public IReadOnlyCollection<DamageTakenGeneral> DamageTakenGenerals => _damageTakenGenerals.AsReadOnly();

    public IReadOnlyCollection<ResourceRecovery> ResourceRecoveries => _resourceRecoveries.AsReadOnly();

    public IReadOnlyCollection<ResourceRecoveryGeneral> ResourceRecoveryGenerals => _resourceRecoveryGenerals.AsReadOnly();

    public IReadOnlyCollection<CombatPlayerDeath> CombatPlayerDeathes => _combatPlayerDeathes.AsReadOnly();

    public IReadOnlyCollection<CombatPlayerPosition> CombatPlayerPositions => _combatPlayerPositions.AsReadOnly();

    public static CombatPlayer Create(double averageItemLevel, int resourcesRecovery, int damageDone, int healDone, int damageTaken,
        string playerId, int combatId, CombatPlayerStatsData stats, SpecializationScoreData score, IReadOnlyList<DamageDoneData> damageDones, 
        IReadOnlyList<DamageDoneGeneralData> damageDoneGenerals, IReadOnlyList<HealDoneData> healDones, IReadOnlyList<HealDoneGeneralData> healDoneGenerals, IReadOnlyList<DamageTakenData> damageTakens, IReadOnlyList<DamageTakenGeneralData> damageTakenGenerals,
        IReadOnlyList<ResourceRecoveryData> resourceRecoveries, IReadOnlyList<ResourceRecoveryGeneralData> resourceRecoveryGenerals, IReadOnlyList<CombatPlayerDeathData> combatPlayerDeathes, IReadOnlyCollection<CombatPlayerPositionData> combatPlayerPositions)
    {
        ArgumentException.ThrowIfNullOrEmpty(playerId, nameof(playerId));
        ArgumentOutOfRangeException.ThrowIfNegative(averageItemLevel, nameof(averageItemLevel));
        ArgumentOutOfRangeException.ThrowIfNegative(resourcesRecovery, nameof(resourcesRecovery));
        ArgumentOutOfRangeException.ThrowIfNegative(damageDone, nameof(damageDone));
        ArgumentOutOfRangeException.ThrowIfNegative(healDone, nameof(healDone));
        ArgumentOutOfRangeException.ThrowIfNegative(damageTaken, nameof(damageTaken));

        var combatPlayer = new CombatPlayer(averageItemLevel, resourcesRecovery, damageDone, healDone, damageTaken, 
            playerId, combatId);

        combatPlayer.AddStats(stats);
        //combatPlayer.AddSpecializationScore(score);

        foreach (var damage in damageDones)
        {
            combatPlayer.AddDamageDone(damage);
        }

        foreach (var damageGeneral in damageDoneGenerals)
        {
            combatPlayer.AddDamageDoneGeneral(damageGeneral);
        }

        foreach (var heal in healDones)
        {
            combatPlayer.AddHealDone(heal);
        }

        foreach (var healGeneral in healDoneGenerals)
        {
            combatPlayer.AddHealDoneGeneral(healGeneral);
        }

        foreach (var damageTakenDone in damageTakens)
        {
            combatPlayer.AddDamageTaken(damageTakenDone);
        }

        foreach (var damageTakenDoneGeneral in damageTakenGenerals)
        {
            combatPlayer.AddDamageTakenGeneral(damageTakenDoneGeneral);
        }

        foreach (var resourceRecovery in resourceRecoveries)
        {
            combatPlayer.AddResourceRecovery(resourceRecovery);
        }

        foreach (var resourceRecoveryGeneral in resourceRecoveryGenerals)
        {
            combatPlayer.AddResourceRecoveryGeneral(resourceRecoveryGeneral);
        }

        foreach (var combatPlayerDeath in combatPlayerDeathes)
        {
            combatPlayer.AddCombatPlayerDeath(combatPlayerDeath);
        }

        foreach (var combatPlayerPosition in combatPlayerPositions)
        {
            combatPlayer.AddCombatPlayerPosition(combatPlayerPosition);
        }

        return combatPlayer;
    }

    private void AddDamageDone(DamageDoneData damageDone)
    {
        var createdDamageDone = new DamageDone(damageDone.GameSpellId, damageDone.Spell, damageDone.Value, damageDone.Time, damageDone.Creator,
            damageDone.Target, damageDone.IsTargetBoss, damageDone.DamageType, damageDone.IsPeriodicDamage, damageDone.IsSingleTarget,
            damageDone.IsPet, damageDone.CombatPlayerId);
        _damageDones.Add(createdDamageDone);
    }

    private void AddDamageDoneGeneral(DamageDoneGeneralData damageDoneGeneral)
    {
        var createdDamageDoneGeneral = new DamageDoneGeneral(damageDoneGeneral.GameSpellId, damageDoneGeneral.Spell, damageDoneGeneral.Value, damageDoneGeneral.DamagePerSecond, damageDoneGeneral.CritNumber,
            damageDoneGeneral.MissNumber, damageDoneGeneral.CastNumber, damageDoneGeneral.MinValue, damageDoneGeneral.MaxValue, damageDoneGeneral.AverageValue,
            damageDoneGeneral.IsPet, damageDoneGeneral.CombatPlayerId);
        _damageDoneGenerals.Add(createdDamageDoneGeneral);
    }

    private void AddHealDone(HealDoneData healDone)
    {
        var createdHealDone = new HealDone(healDone.GameSpellId, healDone.Spell, healDone.Value, healDone.Time, healDone.Creator,
            healDone.Target, healDone.Overheal, healDone.IsCrit, healDone.IsAbsorbed, healDone.CombatPlayerId);
        _healDones.Add(createdHealDone);
    }

    private void AddHealDoneGeneral(HealDoneGeneralData healDoneGeneral)
    {
        var createdHealDoneGeneral = new HealDoneGeneral(healDoneGeneral.GameSpellId, healDoneGeneral.Spell, healDoneGeneral.Value, healDoneGeneral.HealPerSecond, healDoneGeneral.CritNumber,
            healDoneGeneral.CastNumber, healDoneGeneral.MinValue, healDoneGeneral.MaxValue, healDoneGeneral.AverageValue, healDoneGeneral.CombatPlayerId);
        _healDoneGenerals.Add(createdHealDoneGeneral);
    }

    private void AddDamageTaken(DamageTakenData damageTaken)
    {
        var createdDamageTaken = new DamageTaken(damageTaken.GameSpellId, damageTaken.Spell, damageTaken.Value, damageTaken.Time, damageTaken.Creator,
            damageTaken.Target, damageTaken.DamageTakenType, damageTaken.ActualValue, damageTaken.IsPeriodicDamage, damageTaken.Resisted,
            damageTaken.Absorbed, damageTaken.Blocked, damageTaken.RealDamage, damageTaken.Mitigated, damageTaken.CombatPlayerId);
        _damageTakens.Add(createdDamageTaken);
    }

    private void AddDamageTakenGeneral(DamageTakenGeneralData damageTakenGeneral)
    {
        var createdDamageTakenGeneral = new DamageTakenGeneral(damageTakenGeneral.GameSpellId, damageTakenGeneral.Spell, damageTakenGeneral.Value, damageTakenGeneral.ActualValue, damageTakenGeneral.DamageTakenPerSecond,
            damageTakenGeneral.MissNumber, damageTakenGeneral.CritNumber, damageTakenGeneral.CastNumber, damageTakenGeneral.MinValue, damageTakenGeneral.MaxValue, 
            damageTakenGeneral.AverageValue, damageTakenGeneral.CombatPlayerId);
        _damageTakenGenerals.Add(createdDamageTakenGeneral);
    }

    private void AddResourceRecovery(ResourceRecoveryData resourceRecovery)
    {
        var createdResourceRecovery = new ResourceRecovery(resourceRecovery.GameSpellId, resourceRecovery.Spell, resourceRecovery.Value, resourceRecovery.Time, resourceRecovery.Creator,
            resourceRecovery.Target, resourceRecovery.CombatPlayerId);
        _resourceRecoveries.Add(createdResourceRecovery);
    }

    private void AddResourceRecoveryGeneral(ResourceRecoveryGeneralData resourceGeneral)
    {
        var createdResourceRecoveryGeneral = new ResourceRecoveryGeneral(resourceGeneral.GameSpellId, resourceGeneral.Spell, resourceGeneral.Value, resourceGeneral.ResourcePerSecond,
            resourceGeneral.CastNumber, resourceGeneral.MinValue, resourceGeneral.MaxValue, resourceGeneral.AverageValue, resourceGeneral.CombatPlayerId);
        _resourceRecoveryGenerals.Add(createdResourceRecoveryGeneral);
    }

    private void AddCombatPlayerDeath(CombatPlayerDeathData death)
    {
        var createdCombatPlayerDeath = new CombatPlayerDeath(death.Username, death.LastHitSpell, death.LastHitValue, death.Time, death.CombatPlayerId);
        _combatPlayerDeathes.Add(createdCombatPlayerDeath);
    }

    private void AddCombatPlayerPosition(CombatPlayerPositionData position)
    {
        var createdPosition = new CombatPlayerPosition(position.PositionX, position.PositionY, position.Time, position.CombatPlayerId, position.CombatId);
        _combatPlayerPositions.Add(createdPosition);
    }

    private void AddStats(CombatPlayerStatsData stats)
    {
        var createdStats = new CombatPlayerStats(stats.Strength, stats.Agility, stats.Intelligence, stats.Stamina, stats.Spirit, 
            stats.Dodge, stats.Parry, stats.Crit, stats.Haste, stats.Hit,
            stats.Expertise, stats.Armor, stats.Talents, stats.CombatPlayerId);
        Stats = createdStats;
    }

    private void AddSpecializationScore(SpecializationScoreData score)
    {
        var createdScore = new SpecializationScore(score.DamageScore, score.DamageDone, score.HealScore, score.HealDone, score.Updated, 
            score.SpecializationId, score.CombatPlayerId);
        Score = createdScore;
    }
}
