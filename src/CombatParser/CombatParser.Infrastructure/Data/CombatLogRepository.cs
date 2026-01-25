using CombatParser.Domain.Aggregates;
using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Exceptions;
using CombatParser.Infrastructure.Persistence;

namespace CombatParser.Infrastructure.Data;

internal class CombatLogRepository(CombatParserContextOne context) : GenericRepository<CombatLog, int>(context), ICombatLogRepository
{
    private readonly CombatParserContextOne _context = context;

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        var entity = await _context.Set<CombatLog>().FindAsync(id, cancelationToken)
            ?? throw new EntityNotFoundException(typeof(CombatLog), id);

        _context.Set<CombatLog>().Remove(entity);
    }
}
