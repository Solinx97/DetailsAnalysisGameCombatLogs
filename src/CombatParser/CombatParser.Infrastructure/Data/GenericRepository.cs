using CombatParser.Domain.Data;
using CombatParser.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CombatParser.Infrastructure.Data;

internal class GenericRepository<TModel, TId>(CombatParserContext context) : IGenericRepository<TModel, TId>
    where TModel : class
    where TId : notnull
{
    private readonly CombatParserContext _context = context;

    public async Task AddAsync(TModel item, CancellationToken cancelationToken)
    {
        await _context.Set<TModel>().AddAsync(item, cancelationToken);
    }

    public async Task<IEnumerable<TModel>> GetAllAsync(CancellationToken cancelationToken)
    {
        var result = await _context.Set<TModel>()
            .AsNoTracking()
            .ToListAsync(cancelationToken);
        return result.Count != 0 ? result : [];
    }

    public async Task<TModel?> GetByIdAsync(TId id, CancellationToken cancelationToken)
    {
        var entity = await _context.Set<TModel>().FindAsync(id, cancelationToken);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }
}
