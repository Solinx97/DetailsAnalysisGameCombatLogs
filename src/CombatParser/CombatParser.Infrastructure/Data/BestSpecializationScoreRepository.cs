using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class BestSpecializationScoreRepository(CombatParserContextOne context) : IBestSpecializationScoreRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<BestSpecializationScore?> GetAsync(int specializationId, int bossId, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<BestSpecializationScore>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SpecializationId == specializationId && x.BossId == bossId, cancellationToken);

        return entity;
    }
}
