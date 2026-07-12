using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface ISpecializationRepository
{
    Task<Specialization?> GetBySpellsAsync(string spells, CancellationToken cancellationToken);
}
