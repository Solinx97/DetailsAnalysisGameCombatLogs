using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;

namespace CombatAnalysis.WoW.CombatParser.Entities;

public class Unit
{
    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public UnitInfo UnitInfo { get; set; } = new();

    public List<UnitHealth> UnitHealthes { get; set; } = [];

    public List<UnitCast> UnitCasts { get; set; } = [];

    public List<UnitPosition> UnitPositions { get; set; } = [];

    public List<UnitPreAura> PreAuras { get; set; } = [];

    public List<UnitAura> Auras { get; set; } = [];

    public List<DamageDone> DamageDones { get; set; } = [];

    public List<DamageDoneGeneral> DamageDoneGenerals { get; set; } = [];

    public List<DamageDone> DamageTakens { get; set; } = [];

    public List<DamageDoneGeneral> DamageTakenGenerals { get; set; } = [];

    public List<HealDone> HealDones { get; set; } = [];

    public List<HealDoneGeneral> HealDoneGenerals { get; set; } = [];

    public List<ResourceRecovery> ResourceRecoveries { get; set; } = [];

    public List<ResourceRecoveryGeneral> ResourceRecoveryGenerals { get; set; } = [];
}
