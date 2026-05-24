using CombatParser.Domain.Data;
using CombatParser.Domain.Entities.CombatPlayerData;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatPlayerAuraRepository(CombatParserContextOne context) : ICombatPlayerAuraRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<IEnumerable<CombatPlayerAura>> GetByCombatIdAsync(int combatId, CancellationToken cancellationToken)
    {
        var data = await _context.Set<CombatPlayerAura>()
            .AsNoTracking()
            .Where(x => x.CombatPlayer.CombatId == combatId)
            .ToListAsync(cancellationToken);

        return data;
    }

    public async Task<CombatPlayerAura?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var data = await _context.Set<CombatPlayerAura>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return data;
    }
}
