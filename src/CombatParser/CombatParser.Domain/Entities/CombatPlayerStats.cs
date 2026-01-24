using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities;

public class CombatPlayerStats
{
    public int Id { get; set; }

    public int Strength { get; set; }

    public int Agility { get; set; }

    public int Intelligence { get; set; }

    public int Stamina { get; set; }

    public int Spirit { get; set; }

    public int Dodge { get; set; }

    public int Parry { get; set; }

    public int Crit { get; set; }

    public int Haste { get; set; }

    public int Hit { get; set; }

    public int Expertise { get; set; }

    public int Armor { get; set; }

    [MaxLength(126)]
    public string Talents { get; set; } = string.Empty;

    public CombatPlayer CombatPlayer { get; set; }

    public int CombatPlayerId { get; set; }
}
