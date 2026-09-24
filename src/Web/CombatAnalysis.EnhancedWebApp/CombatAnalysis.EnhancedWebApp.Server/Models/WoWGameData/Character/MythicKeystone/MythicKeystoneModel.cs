using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.MythicKeystone;

public class MythicKeystoneModel
{
    [JsonPropertyName("current_period")]
    public MythicKeystoneCurrentPeriodModel CurrentPeriod { get; set; }

    [JsonPropertyName("seasons")]
    public MythicKeystoneSeasonModel[] Seasons { get; set; }

    [JsonPropertyName("character")]
    public DungeonCharacterModel Character { get; set; }

    [JsonPropertyName("current_mythic_rating")]
    public MythicKeystoneRaitingModel CurrentMythicRating { get; set; }
}
