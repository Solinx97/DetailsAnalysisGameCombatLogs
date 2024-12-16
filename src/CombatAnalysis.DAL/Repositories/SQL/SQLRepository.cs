using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

internal class SQLRepository<TModel> : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserSQLContext _context;

    public SQLRepository(CombatParserSQLContext context)
    {
        _context = context;
    }

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var model = Activator.CreateInstance<TModel>();
        model.Id = id;

        _context.Set<TModel>().Remove(model);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync() => await _context.Set<TModel>().AsNoTracking().ToListAsync();

    public async Task<TModel> GetByIdAsync(int id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public async Task<IEnumerable<TModel>> GetByParamAsync(string paramName, object value)
    {
        var data = await _context.Set<TModel>()
                    .Where(x => EF.Property<object>(x, paramName).Equals(value))
                    .ToListAsync();

        return data;
    }

    public async Task<int> UpdateAsync(TModel item)
    {
        _context.Entry(item).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}
