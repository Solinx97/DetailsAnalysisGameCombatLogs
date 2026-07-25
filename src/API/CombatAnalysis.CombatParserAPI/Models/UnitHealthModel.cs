namespace CombatAnalysis.CombatParserAPI.Models;

public class UnitHealthModel
{
    public string Id { get; set; } = string.Empty;

    public string GamePlayerId { get; set; } = string.Empty;

    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }

    public TimeSpan Time { get; set; }

    public bool IsDead { get; set; }

    public int CombatId { get; set; }
}
