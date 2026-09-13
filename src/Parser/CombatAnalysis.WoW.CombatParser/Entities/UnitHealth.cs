namespace CombatAnalysis.WoW.CombatParser.Entities;

public class UnitHealth
{
    public string OwnerGameId { get; set; } = string.Empty;

    public long CurrentHealth { get; set; }

    public long MaxHealth { get; set; }

    public TimeSpan Time { get; set; }
}
