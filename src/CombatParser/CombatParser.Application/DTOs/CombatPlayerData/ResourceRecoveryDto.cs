namespace CombatParser.Application.DTOs.CombatPlayerData;

public class ResourceRecoveryDto
{
    public string Id { get; set; }

    public int GameSpellId { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public TimeSpan Time { get; set; }

    public UnitDto Creator { get; set; } = new();

    public UnitDto Target { get; set; } = new();

    public int ModificationType { get; set; }

    public string UnitId { get; set; }
}
