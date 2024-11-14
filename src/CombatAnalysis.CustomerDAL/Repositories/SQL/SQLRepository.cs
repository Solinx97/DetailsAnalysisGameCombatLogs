using CombatAnalysis.CustomerDAL.Data;
using CombatAnalysis.CustomerDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CustomerDAL.Repositories.SQL;

internal class SQLRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    private readonly CustomerSQLContext _context;

    public SQLRepository(CustomerSQLContext context)
    {
        _context = context;
    }

    public async Task<TModel> CreateAsync(TModel item)
    {
        var entityEntry = await _context.Set<TModel>().AddAsync(item);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
    }

    public async Task<int> DeleteAsync(TIdType id)
    {
        var model = Activator.CreateInstance<TModel>();
        model.GetType().GetProperty("Id").SetValue(model, id);

        _context.Set<TModel>().Remove(model);
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
        var collection = _context.Set<TModel>().AsNoTracking().AsEnumerable();
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
