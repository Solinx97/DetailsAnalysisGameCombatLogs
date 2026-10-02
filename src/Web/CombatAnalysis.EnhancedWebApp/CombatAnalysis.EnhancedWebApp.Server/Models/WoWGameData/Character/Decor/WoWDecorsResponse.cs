using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Decor;

public class WoWDecorsResponse : IWoWCollectionResponse
{
    [JsonPropertyName("decor_items")]
    public WoWGameDataEntityModel[] Items { get; set; }
}
