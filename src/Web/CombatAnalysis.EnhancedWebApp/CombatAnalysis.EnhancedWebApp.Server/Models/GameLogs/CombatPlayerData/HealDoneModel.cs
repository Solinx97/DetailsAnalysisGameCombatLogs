namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

public class HealDoneModel
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public int Overheal { get; set; }

    public string Time { get; set; }

    public UnitModel Creator { get; set; } = new();

    public UnitModel Target { get; set; } = new();

    public int ModificationType { get; set; }

    public int CombatPlayerId { get; set; }
}
