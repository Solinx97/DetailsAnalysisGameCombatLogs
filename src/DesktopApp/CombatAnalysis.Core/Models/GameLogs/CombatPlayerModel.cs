using CombatAnalysis.CombatParser.Entities;

namespace CombatAnalysis.Core.Models.GameLogs;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public int DamageDoneToBoss { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public int ResourcesRecovery { get; set; }

    public CombatPlayerStatsModel Stats { get; set; }

    public SpecializationScoreModel? Score { get; set; }

    public PlayerModel Player { get; set; }

    public string PlayerId { get; set; }

    public int CombatId { get; set; }

    public double DamageDonePerSecond { get; set; }

    public double HealDonePerSecond { get; set; }

    public double DamageTakenPerSecond { get; set; }

    public double ResourcesRecoveryPerSecond { get; set; }

    public double DamageDonePercentages { get; set; }

    public double HealDonePercentages { get; set; }

    public double DamageTakenPercentages { get; set; }

    public double ResourcesRecoveryPercentages { get; set; }

    public IReadOnlyList<CombatPlayerPreAura> PreAuras { get; set; } = [];

    public IReadOnlyList<CombatPlayerAura> Auras { get; set; } = [];

    public IReadOnlyList<DamageDoneModel> DamageDones { get; set; } = [];

    public IReadOnlyList<DamageDoneGeneralModel> DamageDoneGenerals { get; set; } = [];

    public IReadOnlyList<HealDoneModel> HealDones { get; set; } = [];

    public IReadOnlyList<HealDoneGeneralModel> HealDoneGenerals { get; set; } = [];

    public IReadOnlyList<DamageTakenModel> DamageTakens { get; set; } = [];

    public IReadOnlyList<DamageTakenGeneralModel> DamageTakenGenerals { get; set; } = [];

    public IReadOnlyList<ResourceRecoveryModel> ResourceRecoveries { get; set; } = [];

    public IReadOnlyList<ResourceRecoveryGeneralModel> ResourceRecoveryGenerals { get; set; } = [];

    public IReadOnlyCollection<CombatPlayerDeathModel> CombatPlayerDeathes { get; set; } = [];

    public IReadOnlyCollection<CombatPlayerPosition> CombatPlayerPositions { get; set; } = [];
}
