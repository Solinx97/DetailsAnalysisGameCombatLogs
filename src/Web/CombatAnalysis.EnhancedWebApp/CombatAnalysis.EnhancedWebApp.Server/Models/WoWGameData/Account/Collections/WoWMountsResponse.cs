using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWMountsResponse : IWoWCollectionResponse
{
    [JsonPropertyName("mounts")]
    public WoWGameDataEntityModel[] Items { get; set; }
}
