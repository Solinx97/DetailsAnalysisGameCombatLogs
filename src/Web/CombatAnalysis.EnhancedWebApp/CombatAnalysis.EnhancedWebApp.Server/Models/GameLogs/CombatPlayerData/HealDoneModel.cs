namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

public class HealDoneModel
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public int Overheal { get; set; }

    public string Time { get; set; }

    public CombatUnitModel Creator { get; set; } = new();

    public CombatUnitModel Target { get; set; } = new();

    public bool IsCrit { get; set; }

    public bool IsAbsorbed { get; set; }

    public int CombatPlayerId { get; set; }
}
