using CombatAnalysis.WoW_12_1_0.CombatParser.Entities.CombatPlayerData;

namespace CombatAnalysis.WoW_12_1_0.CombatParser.Entities;

public class CombatPlayer
{
    public double AverageItemLevel { get; set; }

    public int DamageDoneToBoss { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public CombatPlayerStats Stats { get; set; } = new();

    public Player Player { get; set; } = new();

    public int CombatId { get; set; }

    public List<CombatPlayerPreAura> PreAuras { get; set; } = [];

    public List<CombatPlayerAura> Auras { get; set; } = [];

    public List<DamageDone> DamageDones { get; set; } = [];

    public List<DamageDoneGeneral> DamageDoneGenerals { get; set; } = [];

    public List<HealDone> HealDones { get; set; } = [];

    public List<HealDoneGeneral> HealDoneGenerals { get; set; } = [];

    public List<DamageTaken> DamageTakens { get; set; } = [];

    public List<DamageTakenGeneral> DamageTakenGenerals { get; set; } = [];

    public List<ResourceRecovery> ResourceRecoveries { get; set; } = [];

    public List<ResourceRecoveryGeneral> ResourceRecoveryGenerals { get; set; } = [];

    public List<CombatPlayerDeath> CombatPlayerDeathes { get; set; } = [];

    public List<UnitPosition> CombatPlayerPositions { get; set; } = [];
}
