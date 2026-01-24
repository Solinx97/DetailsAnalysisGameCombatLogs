using CombatParser.Domain.Entities.CombatPlayerData;

namespace CombatParser.Domain.Entities;

public class CombatPlayer
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public int ResourcesRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public CombatPlayerStats Stats { get; set; }

    public SpecializationScore? Score { get; set; }

    public Player Player { get; set; }

    public string PlayerId { get; set; } = string.Empty;

    public Combat Combat { get; set; }

    public int CombatId { get; set; }

    public IEnumerable<DamageDone> DamageDones { get; set; }

    public IEnumerable<DamageDoneGeneral> DamageDoneGenerals { get; set; }

    public IEnumerable<HealDone> HealDones { get; set; }

    public IEnumerable<HealDoneGeneral> HealDoneGenerals { get; set; }

    public IEnumerable<DamageTaken> DamageTakens { get; set; }

    public IEnumerable<DamageTakenGeneral> DamageTakenGenerals { get; set; }

    public IEnumerable<CombatPlayerDeath> CombatPlayerDeathes { get; set; }

    public IEnumerable<ResourceRecovery> ResourceRecoveries { get; set; }

    public IEnumerable<ResourceRecoveryGeneral> ResourceRecoveryGenerals { get; set; }

    public IEnumerable<CombatPlayerPosition> CombatPlayerPositions { get; set; }
}
