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

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>()
            .AddAsync(item);

        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

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

    public async Task<IEnumerable<TModel>> GetByPaginationAsync(int page, int pageSize)
    {
        var collection = await _context.Set<TModel>()
            .AsNoTracking()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return collection;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
