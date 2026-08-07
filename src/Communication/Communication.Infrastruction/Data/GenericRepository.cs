using Communication.Domain.Data;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class GenericRepository<TModel, TId>(CommunicationContext context) : IGenericRepository<TModel, TId>
    where TModel : class
    where TId : notnull
{
    private readonly CommunicationContext _context = context;

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

    public async Task<IEnumerable<TModel>> GetAsync(int page, int pageSize, CancellationToken cancelationToken)
    {
        var result = await _context.Set<TModel>()
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancelationToken);
        return result.Count != 0 ? result : [];
    }

    public async Task<TModel> GetByIdAsync(TId id, CancellationToken cancelationToken)
    {
        var entity = await _context.Set<TModel>()
            .FindAsync(id, cancelationToken)
            ?? throw new EntityNotFoundException(typeof(TModel), id);

        return entity;
    }
}
