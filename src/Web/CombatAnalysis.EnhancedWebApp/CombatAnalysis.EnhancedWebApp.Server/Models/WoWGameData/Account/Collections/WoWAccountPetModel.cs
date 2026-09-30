using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Account.Collections;

public class WoWAccountPetModel : IWoWCollectionItemModel
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("species")]
    public WoWGameDataEntityModel Item { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; }

    [JsonPropertyName("quality")]
    public WoWGameDataTypeModel Quality { get; set; }

    [JsonPropertyName("stats")]
    public WoWAccountPetStatModel Stats { get; set; }

    [JsonPropertyName("is_favorite")]
    public bool IsFavorite { get; set; }
}
