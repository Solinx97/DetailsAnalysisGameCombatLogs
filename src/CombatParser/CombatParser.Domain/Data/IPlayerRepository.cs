using CombatParser.Domain.Entities;

namespace CombatParser.Domain.Data;

public interface IPlayerRepository
{
    Task<Player?> GetByGameIdAsync(string gameId, CancellationToken cancellationToken);
}
