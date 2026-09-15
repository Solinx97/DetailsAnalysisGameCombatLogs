using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public CombatPlayerStatsModel Stats { get; set; }

    public SpecializationScoreModel? Score { get; set; }

    public PlayerModel Player { get; set; }

    public UnitModel? Unit { get; set; }

    public string UnitId { get; set; }
}
