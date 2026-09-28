using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardAfixModel
{
    [JsonPropertyName("keystone_affix")]
    public WoWGameDataEntityModel Afix { get; set; }

    [JsonPropertyName("starting_level")]
    public int StartingLevel { get; set; }

    [JsonPropertyName("max_level")]
    public int MaxLevel { get; set; }
}
