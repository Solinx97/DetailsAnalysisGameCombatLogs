using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories
{
    public class GenericRepository<TModel> : IGenericRepository<TModel>
        where TModel : class
    {
        private readonly CombatAnalysisContext _context;

        public GenericRepository(CombatAnalysisContext context)
        {
            _context = context;
        }

        async Task<int> IGenericRepository<TModel>.CreateAsync(TModel item)
        {
            var entityEntry = await _context.Set<TModel>().AddAsync(item);
            await _context.SaveChangesAsync();

            var entityId = (int)entityEntry.Property("Id").CurrentValue;

            return entityId;
        }

        async Task<int> IGenericRepository<TModel>.DeleteAsync(TModel item)
        {
            _context.Set<TModel>().Remove(item);
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel>.GetAllAsync() => await _context.Set<TModel>().AsNoTracking().ToListAsync();

        async Task<TModel> IGenericRepository<TModel>.GetByIdAsync(int id)
        {
            var entity = await _context.Set<TModel>().FindAsync(id);

            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        IEnumerable<TModel> IGenericRepository<TModel>.GetByParam(string paramName, object value)
        {
            var collection = _context.Set<TModel>().AsEnumerable();
            var data = collection.Where(x => x.GetType().GetProperty(paramName).GetValue(x).Equals(value));

            return data;
        }

        async Task<int> IGenericRepository<TModel>.UpdateAsync(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }
    }
}
