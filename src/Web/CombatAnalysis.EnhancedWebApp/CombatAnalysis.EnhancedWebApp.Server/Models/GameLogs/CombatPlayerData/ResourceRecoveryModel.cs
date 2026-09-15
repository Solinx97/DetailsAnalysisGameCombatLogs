namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

public class ResourceRecoveryModel
{
    public string Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public UnitModel Unit { get; set; } = new();

    public UnitModel Target { get; set; } = new();

    public int ModificationType { get; set; }

    public string UnitId { get; set; }
}
