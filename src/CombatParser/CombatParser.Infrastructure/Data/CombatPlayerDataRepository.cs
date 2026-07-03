using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerDataRepository<TModel>(CombatParserContextOne context) : ICombatPlayerDataRepository<TModel>
    where TModel : class, ICombatPlayerRefs, ICombatPlayerTime
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
