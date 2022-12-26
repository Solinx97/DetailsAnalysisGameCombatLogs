using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

public class SQLRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    private readonly SQLContext _context;

    public SQLRepository(SQLContext context)
    {
        _context = context;
    }

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(TModel item)
    {
        _context.Set<TModel>().Remove(item);
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync() => await _context.Set<TModel>().AsNoTracking().ToListAsync();

    public async Task<TModel> GetByIdAsync(TIdType id)
    {
        var entity = await _context.Set<TModel>().FindAsync(id);
        if (entity != null)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        return entity;
    }

    public IEnumerable<TModel> GetByParam(string paramName, object value)
    {
        var collection = _context.Set<TModel>().AsEnumerable();
        var data = collection.Where(x => x.GetType().GetProperty(paramName).GetValue(x).Equals(value));

        return data;
    }

    public async Task<int> UpdateAsync(TModel item)
    {
        _context.Entry(item).State = EntityState.Modified;
        var rowsAffected = await _context.SaveChangesAsync();

        return rowsAffected;
    }
}
