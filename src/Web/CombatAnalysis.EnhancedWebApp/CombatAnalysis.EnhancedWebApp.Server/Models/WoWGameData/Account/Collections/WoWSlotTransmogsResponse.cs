using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWSlotTransmogsResponse : IWoWCollectionResponse
{
    [JsonPropertyName("appearances")]
    public WoWGameDataEntityModel[] Items { get; set; }
}
