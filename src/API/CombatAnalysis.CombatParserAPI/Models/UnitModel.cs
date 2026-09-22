using CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;
using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitModel
{
    public string Id { get; set; } = string.Empty;

    [Required]
    public string GameId { get; set; } = string.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public UnitInfoModel UnitInfo { get; set; } = new();

    [Range(0, int.MaxValue)]
    public int CombatId { get; set; }

    [Required]
    public IReadOnlyList<UnitHealthModel> UnitHealthes { get; set; } = [];

    [Required]
    public IReadOnlyList<UnitCastModel> UnitCasts { get; init; } = [];

    [Required]
    public IReadOnlyList<UnitPositionModel> UnitPositions { get; init; } = [];

    [Required]
    public IReadOnlyList<UnitPreAuraModel> PreAuras { get; set; } = [];

    [Required]
    public IReadOnlyList<UnitAuraModel> Auras { get; set; } = [];

    [Required]
    public IReadOnlyList<DamageDoneModel> DamageDones { get; set; } = [];

    [Required]
    public IReadOnlyList<DamageDoneGeneralModel> DamageDoneGenerals { get; set; } = [];

    [Required]
    public IReadOnlyList<HealDoneModel> HealDones { get; set; } = [];

    [Required]
    public IReadOnlyList<HealDoneGeneralModel> HealDoneGenerals { get; set; } = [];

    [Required]
    public IReadOnlyList<ResourceRecoveryModel> ResourceRecoveries { get; set; } = [];

    [Required]
    public IReadOnlyList<ResourceRecoveryGeneralModel> ResourceRecoveryGenerals { get; set; } = [];
}
