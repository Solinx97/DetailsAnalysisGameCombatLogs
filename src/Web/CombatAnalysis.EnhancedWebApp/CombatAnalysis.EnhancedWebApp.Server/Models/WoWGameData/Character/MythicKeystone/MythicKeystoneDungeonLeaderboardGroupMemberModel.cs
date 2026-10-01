using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneDungeonLeaderboardGroupMemberModel
{
    [JsonPropertyName("profile")]
    public WoWGameDataCharacterModel Character { get; set; }

    [JsonPropertyName("faction")]
    public WoWGameDataTypeModel Faction { get; set; }

    [JsonPropertyName("specialization")]
    public WoWGameDataEntityModel Specialization { get; set; }
}
