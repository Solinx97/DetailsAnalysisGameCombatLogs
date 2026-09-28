using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardModel
{
    [JsonPropertyName("map")]
    public WoWGameDataEntityModel Map { get; set; }

    [JsonPropertyName("period")]
    public int Period { get; set; }

    [JsonPropertyName("period_start_timestamp")]
    public long PeriodStartTimestamp { get; set; }

    [JsonPropertyName("period_end_timestamp")]
    public long PeriodEndTimestamp { get; set; }

    [JsonPropertyName("leading_groups")]
    public MythicKeystoneDungeonLeaderboardGroupModel[] LeadingGroups { get; set; }

    [JsonPropertyName("keystone_affixes")]
    public MythicKeystoneDungeonLeaderboardAfixModel[] Afixes { get; set; }

    [JsonPropertyName("map_challenge_mode_id")]
    public int MapChallengeModeId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }
}
