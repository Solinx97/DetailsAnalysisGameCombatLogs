using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountMountModel
{
    [JsonPropertyName("mount")]
    public WoWGameDataEntityModel Mount { get; set; }

    [JsonPropertyName("is_favorite")]
    public bool IsFavorite { get; set; }
}
