namespace CombatParser.Application.DTOs.CombatPlayerData;

public class DamageDoneDto
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public CombatUnitDto Creator { get; set; } = new();

    public CombatUnitDto Target { get; set; } = new();

    public int ModificationType { get; set; }

    public int DamageType { get; set; }

    public int Resisted { get; set; }

    public int Absorbed { get; set; }

    public int Blocked { get; set; }

    public int RealDamage { get; set; }

    public int Overkill { get; set; }

    public int Mitigated { get; set; }

    public int CombatPlayerId { get; set; }
}
