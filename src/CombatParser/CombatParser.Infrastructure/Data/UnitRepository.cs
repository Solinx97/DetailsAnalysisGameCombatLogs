using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class UnitRepository<TModel>(CombatParserContextOne context) : IUnitRepository<TModel>
    where TModel : class, ICombatRefs, IUnitRef, ITime
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IDictionary<string, IEnumerable<TModel>>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
                    .AsNoTracking()
                    .Where(x => x.CombatId == combatId)
                    .GroupBy(x => x.CreatorGameId)
                    .ToDictionaryAsync(x => x.Key, x => x.OrderBy(y => y.Time).Select(y => y), cancellationToken);

        return data.Count != 0 ? data : [];
    }
}
