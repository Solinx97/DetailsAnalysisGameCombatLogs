using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
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

        async Task<int> IGenericRepository<TModel>.UpdateAsync(TModel item)
        {
            _context.Entry(item).State = EntityState.Modified;
            var numberEntries = await _context.SaveChangesAsync();

            return numberEntries;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel>.GetByProcedureAsync(string procedureName, string[] paramNames, object[] paramValuee)
        {
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();
            for (int i = 0; i < paramValuee.Length; i++)
            {
                var paramValue = (int)paramValuee[i];
                procedureParams.Add(new SqlParameter(paramNames[i], paramValue));
                procedureParamNames.Append($"@{paramNames[i]},");
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var data = await _context.Set<TModel>()
                                .FromSqlRaw($"{procedureName} {procedureParamNames}", procedureParams.ToArray())
                                .ToListAsync();

            return data;
        }

        async Task<int> IGenericRepository<TModel>.DeleteByProcedureAsync(string procedureName, string[] paramNames, object[] paramValuee)
        {
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();
            for (int i = 0; i < paramValuee.Length; i++)
            {
                var paramValue = (int)paramValuee[i];
                procedureParams.Add(new SqlParameter(paramNames[i], paramValue));
                procedureParamNames.Append($"@{paramNames[i]},");
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var data = await _context.Database
                                .ExecuteSqlRawAsync($"{procedureName} {procedureParamNames}", procedureParams.ToArray());

            return data;
        }
    }
}
