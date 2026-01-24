using System.ComponentModel.DataAnnotations;

namespace CombatParser.Domain.Entities;

public class Boss
{
    public int Id { get; set; }

    public int GameId { get; set; }

    [MaxLength(126)]
    public string Name { get; set; } = string.Empty;

    public long Health { get; set; }

    public int Difficult { get; set; }

    public int Size { get; set; }

    public ICollection<BestSpecializationScore> BestSpecializationScores { get; set; } = [];
}
