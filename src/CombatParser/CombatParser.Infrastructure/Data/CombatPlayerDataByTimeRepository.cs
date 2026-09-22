using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerDataByTimeRepository<TModel>(CombatParserContextOne context) : ICombatPlayerDataByTimeRepository<TModel>
    where TModel : class, ICombatUnitRefs, ITime
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .OrderBy(x => x.Time)
            .ToListAsync(cancellationToken);

        return data;
    }
}
