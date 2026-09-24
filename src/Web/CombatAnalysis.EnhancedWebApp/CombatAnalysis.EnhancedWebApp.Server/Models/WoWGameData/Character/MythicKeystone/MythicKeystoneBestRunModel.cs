using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneBestRunModel
{
    [JsonPropertyName("completed_timestamp")]
    public long CompletedTimestamp { get; set; }

    [JsonPropertyName("duration")]
    public int Duration { get; set; }

    [JsonPropertyName("keystone_level")]
    public int Level { get; set; }

    [JsonPropertyName("keystone_affixes")]
    public MythicKeystoneAfixModel[] Afixes { get; set; }

    [JsonPropertyName("members")]
    public MythicKeystoneMemberModel[] Members { get; set; }

    [JsonPropertyName("dungeon")]
    public MythicKeystoneDungeonModel Dungeon { get; set; }

    [JsonPropertyName("is_completed_within_time")]
    public bool IsCompletedWithinTime { get; set; }

    [JsonPropertyName("mythic_rating")]
    public MythicKeystoneRaitingModel MythicRating { get; set; }

    [JsonPropertyName("map_rating")]
    public MythicKeystoneRaitingModel MapRating { get; set; }
}
