namespace CombatParser.Domain.Entities;

public class Specialization
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SpecializationSpellsId { get; set;  } = string.Empty;

    public ICollection<SpecializationScore> SpecializationScores { get; set; } = [];

    public ICollection<BestSpecializationScore> BestSpecializationScores { get; set; } = [];
}
