using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class CombatLogRepository(CombatParserContextOne context) : GenericRepository<CombatLog, int>(context), ICombatLogRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<CombatLog>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
