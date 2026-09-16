using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.EntityData;

namespace CombatParser.Domain.Entities;

public class Unit : CombatDataBase
{
    public const int GAMEID_MAX_LENGTH = 128;
    public const int NAME_MAX_LENGTH = 128;

    private readonly List<UnitHealth> _unitHealthes = [];
    private readonly List<UnitCast> _unitCasts = [];
    private readonly List<UnitPosition> _unitPositions = [];
    private readonly List<UnitPreAura> _preAuras = [];
    private readonly List<UnitAura> _auras = [];
    private readonly List<DamageDone> _damageDones = [];
    private readonly List<HealDone> _healDones = [];
    private readonly List<ResourceRecovery> _resourceRecoveries = [];

    private Unit() { }

    private Unit(string gameId, string name, string unitHash, int type, string? creatorGameId)
    {
        Id = Guid.NewGuid().ToString();
        GameId = gameId;
        Name = name;
        UnitHash = unitHash;
        Type = type;
        CreatorGameId = creatorGameId;
    }

    public string Id { get; private set; } = string.Empty;

    public string GameId { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string UnitHash { get; private set; }

    public int Type { get; private set; }

    public string? CreatorGameId { get; private set; }

    public UnitInfo UnitInfo { get; private set; }

    public Combat Combat { get; private set; }

    public IReadOnlyCollection<UnitHealth> UnitHealthes => _unitHealthes.AsReadOnly();

    public IReadOnlyCollection<UnitCast> UnitCasts => _unitCasts.AsReadOnly();

    public IReadOnlyCollection<UnitPosition> UnitPositions => _unitPositions.AsReadOnly();

    public IReadOnlyCollection<UnitPreAura> PreAuras => _preAuras.AsReadOnly();

    public IReadOnlyCollection<UnitAura> Auras => _auras.AsReadOnly();

    public IReadOnlyCollection<DamageDone> DamageDones => _damageDones.AsReadOnly();

    public IReadOnlyCollection<HealDone> HealDones => _healDones.AsReadOnly();

    public IReadOnlyCollection<ResourceRecovery> ResourceRecoveries => _resourceRecoveries.AsReadOnly();

    public static Unit Create(string gameId, string name, string unitHash, int type, string? creatorGameId, UnitInfoData unitInfo,
        IReadOnlyList<UnitHealthData> unitHealthes, IReadOnlyList<UnitCastData> unitCasts, IReadOnlyList<UnitPositionData> unitPositions,
        IReadOnlyList<UnitPreAuraData> preAuras, IReadOnlyList<UnitAuraData> auras,
        IReadOnlyList<DamageDoneData> damageDones, IReadOnlyList<HealDoneData> healDones, IReadOnlyList<ResourceRecoveryData> resourceRecoveries)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId, nameof(gameId));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(unitHash, nameof(unitHash));

        var unit = new Unit(gameId, name, unitHash, type, creatorGameId);

        unit.AddUnitInfo(unitInfo);

        foreach (var unitHealth in unitHealthes)
        {
            unit.AddHealth(unitHealth);
        }

        foreach (var unitCast in unitCasts)
        {
            unit.AddCast(unitCast);
        }

        foreach (var unitPosition in unitPositions)
        {
            unit.AddUnitPosition(unitPosition);
        }

        foreach (var preAura in preAuras)
        {
            unit.AddPreAura(preAura);
        }

        foreach (var aura in auras)
        {
            unit.AddAura(aura);
        }

        foreach (var damage in damageDones)
        {
            unit.AddDamageDone(damage);
        }

        foreach (var heal in healDones)
        {
            unit.AddHealDone(heal);
        }

        foreach (var resourceRecovery in resourceRecoveries)
        {
            unit.AddResourceRecovery(resourceRecovery);
        }

        return unit;
    }

    private void AddUnitInfo(UnitInfoData unitInfo)
    {
        var createdUnitInfo = UnitInfo.Create(unitInfo.ResourcesRecovery, unitInfo.DamageDone, unitInfo.HealDone, unitInfo.DamageTaken);
        UnitInfo = createdUnitInfo;
    }

    private void AddHealth(UnitHealthData health)
    {
        var createdHealth = UnitHealth.Create(health.OwnerGameId, health.CurrentHealth, health.MaxHealth, health.Status, health.Time);
        _unitHealthes.Add(createdHealth);
    }

    private void AddCast(UnitCastData cast)
    {
        var createdCast = UnitCast.Create(cast.OwnerGameId, cast.GameSpellId, cast.Spell, cast.Time, cast.FinishTime,
            cast.TargetGameId, cast.IsImmediatly, cast.IsSuccess);
        _unitCasts.Add(createdCast);
    }

    private void AddUnitPosition(UnitPositionData unitPosition)
    {
        var createdPosition = UnitPosition.Create(unitPosition.OwnerGameId, unitPosition.X, unitPosition.Y,
            unitPosition.Time);
        _unitPositions.Add(createdPosition);
    }

    private void AddPreAura(UnitPreAuraData preAura)
    {
        var createdPreAura = UnitPreAura.Create(preAura.GameId, preAura.Status, preAura.TargetGameId);
        _preAuras.Add(createdPreAura);
    }

    private void AddAura(UnitAuraData aura)
    {
        var createdAura = UnitAura.Create(aura.GameAuraId, aura.Name, aura.AuraCreatorType,
            aura.AuraType, aura.StartTime, aura.FinishTime, aura.Stacks, aura.TargetGameId);
        _auras.Add(createdAura);
    }

    private void AddDamageDone(DamageDoneData damageDone)
    {
        var createdDamageDone = DamageDone.Create(damageDone.GameSpellId, damageDone.Spell, damageDone.Value, damageDone.Time,
            damageDone.ModificationType, damageDone.DamageType, damageDone.Resisted, damageDone.Absorbed,
            damageDone.Blocked, damageDone.RealDamage, damageDone.Overkill, damageDone.Mitigated, damageDone.TargetGameId);
        _damageDones.Add(createdDamageDone);
    }

    private void AddHealDone(HealDoneData healDone)
    {
        var createdHealDone = HealDone.Create(healDone.GameSpellId, healDone.Spell, healDone.Value, healDone.Time,
            healDone.Overheal, healDone.ModificationType, healDone.TargetGameId);
        _healDones.Add(createdHealDone);
    }

    private void AddResourceRecovery(ResourceRecoveryData resourceRecovery)
    {
        var createdResourceRecovery = ResourceRecovery.Create(resourceRecovery.GameSpellId, resourceRecovery.Spell, resourceRecovery.Value, resourceRecovery.Time, resourceRecovery.ModificationType, resourceRecovery.TargetGameId);
        _resourceRecoveries.Add(createdResourceRecovery);
    }
}
