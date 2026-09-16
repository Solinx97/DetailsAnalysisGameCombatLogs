using CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;

namespace CombatAnalysis.Core.Models.GameLogs;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public CombatPlayerStatsModel? Stats { get; set; }

    public SpecializationScoreModel? Score { get; set; }

    public PlayerModel Player { get; set; }

    public string PlayerId { get; set; }

    public UnitModel Unit { get; set; }

    public string UnitId { get; set; }

    public int CombatId { get; set; }

    public double DamageDonePerSecond { get; set; }

    public double HealDonePerSecond { get; set; }

    public double DamageTakenPerSecond { get; set; }

    public double ResourcesRecoveryPerSecond { get; set; }

    public double DamageDonePercentages { get; set; }

    public double HealDonePercentages { get; set; }

    public double DamageTakenPercentages { get; set; }

    public double ResourcesRecoveryPercentages { get; set; }
}
