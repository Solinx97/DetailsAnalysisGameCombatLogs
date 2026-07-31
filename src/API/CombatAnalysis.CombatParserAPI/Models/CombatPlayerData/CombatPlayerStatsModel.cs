using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models.CombatPlayerData;

public class CombatPlayerStatsModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int Strength { get; set; }

    [Range(0, int.MaxValue)]
    public int Agility { get; set; }

    [Range(0, int.MaxValue)]
    public int Intelligence { get; set; }

    [Range(0, int.MaxValue)]
    public int Stamina { get; set; }

    [Range(0, int.MaxValue)]
    public int Spirit { get; set; }

    [Range(0, int.MaxValue)]
    public int Dodge { get; set; }

    [Range(0, int.MaxValue)]
    public int Parry { get; set; }

    [Range(0, int.MaxValue)]
    public int Crit { get; set; }

    [Range(0, int.MaxValue)]
    public int Haste { get; set; }

    [Range(0, int.MaxValue)]
    public int Hit { get; set; }

    [Range(0, int.MaxValue)]
    public int Expertise { get; set; }

    [Range(0, int.MaxValue)]
    public int Armor { get; set; }

    [Required]
    public string Talents { get; set; }

    [Range(0, int.MaxValue)]
    public int CombatPlayerId { get; set; }
}
