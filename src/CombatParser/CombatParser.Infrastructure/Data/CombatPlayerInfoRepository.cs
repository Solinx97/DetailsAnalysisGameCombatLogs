using CombatParser.Domain.Data;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerInfoRepository<TModel>(CombatParserContextOne context) : ICombatPlayerInfoRepository<TModel>
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

    public async Task<TModel?> GetFirstByCombatPlayerIdAsync(int combatPlayerId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.CombatPlayerId == combatPlayerId)
            .SingleOrDefaultAsync(cancellationToken);

        return data;
    }
}
