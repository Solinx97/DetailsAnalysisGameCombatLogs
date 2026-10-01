using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneSeasonModel
{
    [JsonPropertyName("best_runs")]
    public MythicKeystoneBestRunModel[] BestRuns { get; set; }

    [JsonPropertyName("season")]
    public WoWGameDataEntityModel Season { get; set; }

    [JsonPropertyName("mythic_rating")]
    public MythicKeystoneRaitingModel MythicRating { get; set; }
}
