using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerAuraRepository(CombatParserContextOne context) : ICombatPlayerAuraRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<UnitAura>> GetAurasAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<Unit>()
            .Join(_context.Set<UnitAura>(),
                x => x.Id,
                y => y.UnitId,
                (x, y) => new
                {
                    CombatId = x.CombatId,
                    Aura = y
                })
            .AsNoTracking()
            .Where(x => x.CombatId == combatId)
            .Select(x => x.Aura)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<IEnumerable<UnitAura>> GetAurasAsync(string unitId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitAura>()
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<UnitAura?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var data = await _context.Set<UnitAura>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return data;
    }
}
