namespace CombatAnalysis.CombatParser.Entities;

public class UnitHealth
{
    public string CreatorGameId { get; set; } = string.Empty;

    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }

    public TimeSpan Time { get; set; }

    public bool IsDead { get; set; }

    public int CombatId { get; set; }
}
