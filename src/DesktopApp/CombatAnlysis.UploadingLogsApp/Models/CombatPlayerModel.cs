using CombatAnalysis.UploadingLogsApp.Interfaces.Entities;

namespace CombatAnalysis.UploadingLogsApp.Models;

public class CombatPlayerModel
{
    public double AverageItemLevel { get; set; }

    public IPlayerStatsModel Stats { get; set; }

    public PlayerModel Player { get; set; }

    public string UnitGameId { get; set; }
}
