using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerDataByTimeRepository<TModel>(CombatParserContextOne context) : ICombatPlayerDataByTimeRepository<TModel>
    where TModel : class, ICombatPlayerRefs, ICombatTime
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.CombatPlayerId == combatPlayerId)
            .OrderBy(x => x.Time)
            .ToListAsync(cancellationToken);

        return data;
    }
}
