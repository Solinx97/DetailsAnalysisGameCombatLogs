using CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs;

public class CombatPlayerModel
{
    public int Id { get; set; }

    public double AverageItemLevel { get; set; }

    public int ResourcesRecovery { get; set; }

    public int DamageDone { get; set; }

    public int HealDone { get; set; }

    public int DamageTaken { get; set; }

    public CombatPlayerStatsModel Stats { get; set; }

    public SpecializationScoreModel? Score { get; set; }

    public PlayerModel Player { get; set; }

    public int CombatId { get; set; }
}
