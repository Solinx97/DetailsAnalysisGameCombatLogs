using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneCurrentPeriodModel
{
    [JsonPropertyName("period")]
    public MythicKeystoneSeasonModel Period { get; set; }

    [JsonPropertyName("best_runs")]
    public MythicKeystoneBestRunModel[] BestRuns { get; set; }
}
