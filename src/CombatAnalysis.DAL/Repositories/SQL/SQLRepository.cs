using CombatAnalysis.DAL.Data.SQL;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.SQL
{
    public class SQLRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        private readonly SQLContext _context;

        public SQLRepository(SQLContext context)
        {
            _context = context;
        }

        async Task<TModel> IGenericRepository<TModel, TIdType>.CreateAsync(TModel item)
        {
            var entityEntry = await _context.Set<TModel>().AddAsync(item);
            await _context.SaveChangesAsync();

            return entityEntry.Entity;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.DeleteAsync(TModel item)
        {
            _context.Set<TModel>().Remove(item);
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel, TIdType>.GetAllAsync() => await _context.Set<TModel>().AsNoTracking().ToListAsync();

        async Task<TModel> IGenericRepository<TModel, TIdType>.GetByIdAsync(TIdType id)
        {
            var entity = await _context.Set<TModel>().FindAsync(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        IEnumerable<TModel> IGenericRepository<TModel, TIdType>.GetByParam(string paramName, object value)
        {
            var collection = _context.Set<TModel>().AsEnumerable();
            var data = collection.Where(x => x.GetType().GetProperty(paramName).GetValue(x).Equals(value));

            return data;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.UpdateAsync(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var rowsAffected = await _context.SaveChangesAsync();

            return rowsAffected;
        }
    }
}
