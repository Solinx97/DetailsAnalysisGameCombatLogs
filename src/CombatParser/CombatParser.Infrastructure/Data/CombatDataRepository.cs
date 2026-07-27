using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatDataRepository<TModel>(CombatParserContextOne context) : ICombatDataRepository<TModel>
    where TModel : class, ICombatRefs
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<TTimeModel>> GetByCombatIdAsync<TTimeModel>(int combatId, CancellationToken cancellationToken)
        where TTimeModel : class, ICombatRefs, ICombatTime
    {
        var data = await _context.Set<TTimeModel>()
            .AsNoTracking()
            .Where(x => x.CombatId == combatId)
            .OrderBy(x => x.Time)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<TModel>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.CombatId == combatId)
            .ToListAsync(cancellationToken);

        return data;
    }
}