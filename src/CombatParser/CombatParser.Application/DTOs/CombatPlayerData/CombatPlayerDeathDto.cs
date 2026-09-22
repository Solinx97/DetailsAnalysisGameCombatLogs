namespace CombatParser.Application.DTOs.CombatPlayerData;

public class CombatPlayerDeathDto
{
    public TimeSpan Time { get; set; }

    public string Name { get; set; }

    public string Spell { get; set; }

    public int Value { get; set; }

    public long CurrentHealth { get; set; }

    public long MaxHealth { get; set; }

    public int Status { get; set; }

    public string UnitId { get; set; }
}
