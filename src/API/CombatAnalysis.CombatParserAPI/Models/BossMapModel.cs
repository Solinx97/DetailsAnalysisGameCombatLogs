using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class BossMapModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public double X0 { get; set; }

    public double X1 { get; set; }

    public double Y0 { get; set; }

    public double Y1 { get; set; }
}
