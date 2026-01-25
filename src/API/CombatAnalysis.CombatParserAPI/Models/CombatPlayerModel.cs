using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public double AverageItemLevel { get; set; }

    [Range(0, int.MaxValue)]
    public int ResourcesRecovery { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageDone { get; set; }

    [Range(0, int.MaxValue)]
    public int HealDone { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageTaken { get; set; }

    public CombatPlayerStatsModel Stats { get; set; }

    public SpecializationScoreModel? Score { get; set; }

    [Required]
    public PlayerModel Player { get; set; }

    [Required]
    public string PlayerId { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }

    public IReadOnlyList<DamageDoneModel> DamageDones { get; set; } = [];

    public IReadOnlyList<DamageDoneGeneralModel> DamageDoneGenerals { get; set; } = [];

    public IReadOnlyList<HealDoneModel> HealDones { get; set; } = [];

    public IReadOnlyList<HealDoneGeneralModel> HealDoneGenerals { get; set; } = [];

    public IReadOnlyList<DamageTakenModel> DamageTakens { get; set; } = [];

    public IReadOnlyList<DamageTakenGeneralModel> DamageTakenGenerals { get; set; } = [];

    public IReadOnlyList<ResourceRecoveryModel> ResourceRecoveries { get; set; } = [];

    public IReadOnlyList<ResourceRecoveryGeneralModel> ResourceRecoveryGenerals { get; set; } = [];

    public IReadOnlyList<CombatPlayerDeathModel> CombatPlayerDeathes { get; set; } = [];

    public IReadOnlyList<CombatPlayerPositionModel> CombatPlayerPositions { get; set; } = [];
}
