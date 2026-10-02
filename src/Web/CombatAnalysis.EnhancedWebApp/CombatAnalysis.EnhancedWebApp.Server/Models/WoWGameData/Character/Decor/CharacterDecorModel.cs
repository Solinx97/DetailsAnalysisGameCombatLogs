using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Decor;

public class CharacterDecorModel
{
    [JsonPropertyName("decor")]
    public WoWGameDataEntityModel Decor { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}
