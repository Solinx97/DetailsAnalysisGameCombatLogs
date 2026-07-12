using System.ComponentModel.DataAnnotations;

namespace CombatAnalysis.CombatParserAPI.Models;

public class BossModel
{
    [Range(0, int.MaxValue)]
    public int Id { get; set; }

    [Range(0, int.MaxValue)]
    public int GameId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public long Health { get; set; }

    [Range(0, int.MaxValue)]
    public int Difficult { get; set; }

    [Range(0, int.MaxValue)]
    public int Size { get; set; }

    [Range(0, int.MaxValue)]
    public int BossMapId { get; set; }
}
