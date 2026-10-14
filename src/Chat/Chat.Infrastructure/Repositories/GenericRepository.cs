using Chat.Domain.Interfaces;
using Chat.Domain.Repositories;
using Chat.Infrastructure.Exceptions;
using Chat.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Repositories;

internal class GenericRepository<TModel, TId>(ChatContext context) : IGenericRepository<TModel, TId>
    where TModel : class, IRepositoryEntity<TId>
    where TId: notnull
{
    protected readonly ChatContext _context = context;

    public async Task DeleteAsync(TId id)
    {
        var entity = await _context.Set<TModel>()
            .SingleOrDefaultAsync(g => g.Id.Equals(id))
                    ?? throw new EntityNotFoundException(typeof(TModel), id);

        _context.Set<TModel>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var collection = await _context.Set<TModel>()
            .AsNoTracking()
            .ToListAsync();

        return collection;
    }

    public async Task<TModel> GetByIdAsync(TId id)
    {
        var entity = await _context.Set<TModel>()
            .SingleOrDefaultAsync(g => g.Id.Equals(id))
                        ?? throw new EntityNotFoundException(typeof(TModel), id);

        return entity;
    }
}
