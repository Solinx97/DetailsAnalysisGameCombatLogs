using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardGroupModel
{
    [JsonPropertyName("ranking")]
    public int Ranking { get; set; }

    [JsonPropertyName("duration")]
    public long Duration { get; set; }

    [JsonPropertyName("completed_timestamp")]
    public long CompletedTimestamp { get; set; }

    [JsonPropertyName("keystone_level")]
    public int KeystoneLevel { get; set; }

    [JsonPropertyName("members")]
    public MythicKeystoneDungeonLeaderboardGroupMemberModel[] Members { get; set; }

    [JsonPropertyName("mythic_rating")]
    public MythicKeystoneRaitingModel MythicRating { get; set; }
}
