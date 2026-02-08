using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerGenericDataRepository<TModel>(CombatParserContextOne context) : ICombatPlayerGenericDataRepository<TModel>
    where TModel : class, ICombatPlayerRefs
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.CombatPlayerId == combatPlayerId)
            .ToListAsync(cancellationToken);

        return data;
    }
}
