using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerDataRepository<TModel>(CombatParserContextOne context) : ICombatPlayerDataRepository<TModel>
    where TModel : class, ICombatPlayerRefs
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.CombatPlayerId == combatPlayerId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<int> CountAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var count = await _context.Set<TModel>()
            .Where(x => x.CombatPlayerId == combatPlayerId)
            .CountAsync(cancellationToken);

        return count;
    }
}
