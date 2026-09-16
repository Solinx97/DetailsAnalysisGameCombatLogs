namespace CombatAnalysis.Core.Models.GameLogs;

public class UnitHealthModel
{
    public string Id { get; set; } = string.Empty;

    public string OwnerGameId { get; set; } = string.Empty;

    public int CurrentHealth { get; set; }

    public int MaxHealth { get; set; }

    public TimeSpan Time { get; set; }

    public string UniId { get; set; }
}
