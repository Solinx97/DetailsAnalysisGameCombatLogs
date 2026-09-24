using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData;

public class RealmModel
{
    [JsonPropertyName("name")]
    public GameDataValueNameModel Name { get; set; }

    [JsonPropertyName("slug")]
    public string Slug { get; set; }
}
