using CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

namespace CombatAnalysis.Core.Models.GameLogs;

public class UnitModel
{
    public string Id { get; set; } = string.Empty;

    public string GameId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string UnitHash { get; set; } = string.Empty;

    public int Type { get; set; }

    public string? CreatorGameId { get; set; }

    public UnitInfoModel UnitInfo { get; set; } = new();

    public int CombatId { get; set; }

    public IReadOnlyList<UnitPreAuraModel> PreAuras { get; set; } = [];

    public IReadOnlyList<UnitAuraModel> Auras { get; set; } = [];

    public IReadOnlyList<DamageDoneModel> DamageDones { get; set; } = [];

    public IReadOnlyList<DamageDoneGeneralModel> DamageDoneGenerals { get; set; } = [];

    public IReadOnlyList<HealDoneModel> HealDones { get; set; } = [];

    public IReadOnlyList<HealDoneGeneralModel> HealDoneGenerals { get; set; } = [];

    public IReadOnlyList<ResourceRecoveryModel> ResourceRecoveries { get; set; } = [];

    public IReadOnlyList<ResourceRecoveryGeneralModel> ResourceRecoveryGenerals { get; set; } = [];

    public IReadOnlyCollection<UnitPositionModel> CombatPlayerPositions { get; set; } = [];
}
