using CombatParser.Domain.Data;
using CombatParser.Domain.Entities;
using CombatParser.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class BossRepository(CombatParserContextOne context) : IBossRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task<Boss?> GetAsync(int gameBossId, int difficult, int groupSize, CancellationToken cancellationToken)
    {
        var boss = await _context.Set<Boss>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.GameId == gameBossId && x.Difficult == difficult && x.Size == groupSize, cancellationToken);

        return boss;
    }
}