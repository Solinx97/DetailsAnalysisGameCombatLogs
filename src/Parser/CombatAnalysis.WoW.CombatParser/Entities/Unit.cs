using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Entities;

public class Unit
{
    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public UnitInfo UnitInfo { get; set; } = new();

    public ConcurrentBag<UnitHealth> UnitHealthes { get; set; } = [];

    public ConcurrentBag<DamageDone> DamageDones { get; set; } = [];

    public ConcurrentBag<DamageDone> DamageTakens { get; set; } = [];

    public ConcurrentBag<HealDone> HealDones { get; set; } = [];

    public ConcurrentBag<ResourceRecovery> ResourceRecoveries { get; set; } = [];

    public List<UnitPreAura> PreAuras { get; set; } = [];

    public List<UnitCast> UnitCasts { get; set; } = [];

    public List<UnitPosition> UnitPositions { get; set; } = [];

    public List<UnitAura> Auras { get; set; } = [];
}
