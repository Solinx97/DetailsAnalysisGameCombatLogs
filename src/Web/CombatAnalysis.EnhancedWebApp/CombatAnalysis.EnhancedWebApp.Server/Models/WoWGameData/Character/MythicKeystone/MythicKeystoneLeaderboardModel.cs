using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneLeaderboardModel
{
    [JsonPropertyName("current_leaderboards")]
    public WoWGameDataEntityModel[] CurrentLeaderboards { get; set; }
}
