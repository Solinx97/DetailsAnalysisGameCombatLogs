using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountTransmogResponse
{
    [JsonPropertyName("appearance_sets")]
    public WoWGameDataEntityModel[] AppearanceSets { get; set; }

    [JsonPropertyName("slots")]
    public WoWAccountTransmogSlotModel[] Slots { get; set; }
}
