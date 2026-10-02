using CombatAnalysis.EnhancedWebApp.Server.Interfaces;
using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountMountModel : IWoWCollectionItemModel
{
    [JsonPropertyName("mount")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("is_favorite")]
    public bool IsFavorite { get; set; }
}
