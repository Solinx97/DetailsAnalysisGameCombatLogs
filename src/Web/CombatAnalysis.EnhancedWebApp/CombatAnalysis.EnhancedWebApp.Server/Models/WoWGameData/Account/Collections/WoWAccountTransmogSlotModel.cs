using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountTransmogSlotModel
{
    [JsonPropertyName("slot")]
    public WoWGameDataTypeModel Slot { get; set; }

    [JsonPropertyName("appearances")]
    public WoWGameDataEntityModel[] Appearances { get; set; }
}
