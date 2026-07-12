using CombatParser.Domain.Aggregates;

namespace CombatParser.Domain.Data;

public interface IBestSpecializationScoreRepository
{
    Task<BestSpecializationScore?> GetAsync(int specializationId, int bossId, CancellationToken cancellationToken);
}
