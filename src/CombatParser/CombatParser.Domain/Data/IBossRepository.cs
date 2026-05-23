using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface IBossRepository
{
    Task<Boss?> GetAsync(int gameBossId, int difficult, int groupSize, CancellationToken cancellationToken);
}
