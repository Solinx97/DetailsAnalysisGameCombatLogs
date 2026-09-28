using System.Text.Json.Serialization;

namespace CombatAnalysis.EnhancedWebApp.Server.Models.WoWGameData.Character.Equipments;

public class CharacterStatPowerModel
{
    [JsonPropertyName("base")]
    public int Base { get; set; }

    [JsonPropertyName("effective")]
    public int Effective { get; set; }
}
