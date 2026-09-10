namespace CombatParser.Application.DTOs.CombatPlayerData;

public class HealDoneDto
{
    public int Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public int Overheal { get; set; }

    public TimeSpan Time { get; set; }

    public CombatUnitDto Creator { get; set; } = new();

    public CombatUnitDto Target { get; set; } = new();

    public bool IsCrit { get; set; }

    public bool IsAbsorbed { get; set; }

    public int CombatPlayerId { get; set; }
}
