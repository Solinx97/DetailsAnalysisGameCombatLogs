using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Domain.Interfaces;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class UnitInfoRepository<TModel>(CombatParserContextOne context) : IUnitInfoRepository<TModel>
    where TModel : class, ICombatUnitRefs
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<DamageDoneGeneral>> GetDamageByCombatPlayerIdAsync(string unitId, bool isPlayerTarget, CancellationToken cancellationToken)
    {
        var data = await _context.Set<DamageDoneGeneral>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId && x.IsPlayerTarget == isPlayerTarget)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<TModel?> GetFirstByCombatPlayerIdAsync(string unitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<TModel>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .SingleOrDefaultAsync(cancellationToken);

        return data;
    }
}
