using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWSetTransmogsResponse : IWoWCollectionResponse
{
    [JsonPropertyName("appearance_sets")]
    public WoWGameDataEntityModel[] Items { get; set; }
}
