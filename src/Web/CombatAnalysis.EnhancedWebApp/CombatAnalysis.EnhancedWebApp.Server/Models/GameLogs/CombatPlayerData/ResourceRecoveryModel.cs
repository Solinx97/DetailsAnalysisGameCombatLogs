namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

public class ResourceRecoveryModel
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public string Time { get; set; }

    public CombatUnitModel Creator { get; set; } = new();

    public CombatUnitModel Target { get; set; } = new();

    public int CombatPlayerId { get; set; }
}
