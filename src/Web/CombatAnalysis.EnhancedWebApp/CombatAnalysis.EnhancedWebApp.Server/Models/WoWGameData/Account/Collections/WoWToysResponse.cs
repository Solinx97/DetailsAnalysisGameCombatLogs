using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWToysResponse : IWoWCollectionResponse
{
    [JsonPropertyName("toys")]
    public WoWGameDataEntityModel[] Items { get; set; }
}
