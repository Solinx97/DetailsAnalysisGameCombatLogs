using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Decor;

public class CharacterDecorsResponse
{
    [JsonPropertyName("decor_collected")]
    public CharacterDecorModel[] DdecorCollected { get; set; }
}
