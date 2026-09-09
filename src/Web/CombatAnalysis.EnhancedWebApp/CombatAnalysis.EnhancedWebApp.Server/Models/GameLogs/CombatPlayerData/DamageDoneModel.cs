namespace CombatAnalysis.EnhancedWebApp.Server.Models.GameLogs.CombatPlayerData;

public class DamageDoneModel
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; } = string.Empty;

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public string CreatorGameId { get; set; } = string.Empty;

    public string TargetGameId { get; set; } = string.Empty;

    public string TargetHash { get; set; } = string.Empty;

    public long TargetCurrentHealth { get; set; }

    public int ModificationType { get; set; }

    public int DamageType { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Mitigated { get; set; }

    public int CombatPlayerId { get; set; }
}
