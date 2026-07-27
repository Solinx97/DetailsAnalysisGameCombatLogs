using CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class CombatPlayerModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public double AverageItemLevel { get; set; }

    public int ResourcesRecovery { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageDone { get; set; }

    [Range(0, int.MaxValue)]
    public int HealDone { get; set; }

    [Range(0, int.MaxValue)]
    public int DamageTaken { get; set; }

    [Required]
    public CombatPlayerStatsModel Stats { get; set; } = new();

    public SpecializationScoreModel? Score { get; set; }

    public PlayerModel Player { get; set; } = new();

    [Required]
    public string PlayerId { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }

    [Required]
    public IReadOnlyList<CombatPlayerPreAuraModel> PreAuras { get; set; } = [];

    [Required]
    public IReadOnlyList<CombatPlayerAuraModel> Auras { get; set; } = [];

    [Required]
    public IReadOnlyList<CombatPlayerCastModel> Casts { get; set; } = [];

    [Required]
    public IReadOnlyList<DamageDoneModel> DamageDones { get; set; } = [];

    [Required]
    public IReadOnlyList<DamageDoneGeneralModel> DamageDoneGenerals { get; set; } = [];

    [Required]
    public IReadOnlyList<HealDoneModel> HealDones { get; set; } = [];

    [Required]
    public IReadOnlyList<HealDoneGeneralModel> HealDoneGenerals { get; set; } = [];

    [Required]
    public IReadOnlyList<DamageTakenModel> DamageTakens { get; set; } = [];

    [Required]
    public IReadOnlyList<DamageTakenGeneralModel> DamageTakenGenerals { get; set; } = [];

    [Required]
    public IReadOnlyList<ResourceRecoveryModel> ResourceRecoveries { get; set; } = [];

    [Required]
    public IReadOnlyList<ResourceRecoveryGeneralModel> ResourceRecoveryGenerals { get; set; } = [];

    [Required]
    public IReadOnlyList<CombatPlayerDeathModel> CombatPlayerDeathes { get; set; } = [];

    [Required]
    public IReadOnlyList<UnitPositionModel> CombatPlayerPositions { get; set; } = [];
}
