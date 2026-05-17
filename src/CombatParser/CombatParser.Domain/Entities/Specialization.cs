using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Entities;

public class Specialization
{
    public const int NAME_MAX_LENGTH = 128;
    public const int SPEC_SPELLS_MAX_LENGTH = 128;

    private Specialization() { }

    public Specialization(int id, string name, string specializationSpellsId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));
        ArgumentException.ThrowIfNullOrEmpty(specializationSpellsId, nameof(specializationSpellsId));

        Id = id;
        Name = name;
        SpecializationSpellsId = specializationSpellsId;
    }

    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string SpecializationSpellsId { get; private set;  } = string.Empty;

    public ICollection<SpecializationScore> SpecializationScores { get; private set; } = [];

    public ICollection<BestSpecializationScore> BestSpecializationScores { get; private set; } = [];
}
