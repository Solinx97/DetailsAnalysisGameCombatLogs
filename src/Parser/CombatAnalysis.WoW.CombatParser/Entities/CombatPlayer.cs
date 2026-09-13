using CombatAnalysis.WoW.CombatParser.Entities.CombatPlayerData;
using CombatAnalysis.WoW.CombatParser.Interfaces.Entities;

namespace CombatAnalysis.WoW.CombatParser.Entities;

public class CombatPlayer
{
    public double AverageItemLevel { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public IPlayerStats Stats { get; set; }

    public Player Player { get; set; } = new();

    public List<CombatPlayerPreAura> PreAuras { get; set; } = [];

    public List<CombatPlayerAura> Auras { get; set; } = [];

    public List<ICombatPlayerResourceRefs> DamageDones { get; set; } = [];

    public List<DamageDoneGeneral> DamageDoneGenerals { get; set; } = [];

    public List<ICombatPlayerResourceRefs> HealDones { get; set; } = [];

    public List<HealDoneGeneral> HealDoneGenerals { get; set; } = [];

    public List<ICombatPlayerResourceRefs> ResourceRecoveries { get; set; } = [];

    public List<ResourceRecoveryGeneral> ResourceRecoveryGenerals { get; set; } = [];
}
