using CombatAnalysis.StoredProcedureDAL.Data;
using CombatAnalysis.StoredProcedureDAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.StoredProcedureDAL.Repositories
{
    public class GenericRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        private readonly CombatAnalysisContext _context;

        public GenericRepository(CombatAnalysisContext context)
        {
            _context = context;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.CreateAsync(string[] paramNames, object[] paramValues)
        {
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();
            for (int i = 0; i < paramValues.Length; i++)
            {
                procedureParams.Add(new SqlParameter(paramNames[i], paramValues[i]));
                procedureParamNames.Append($"@{paramNames[i]},");
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var data = await _context.Database
                                .ExecuteSqlRawAsync($"Delete{nameof(TModel)} {procedureParamNames}", procedureParams);

            return data;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.DeleteAsync(TIdType id)
        {
            var data = await _context.Database
                                .ExecuteSqlRawAsync($"Delete{nameof(TModel)} {nameof(id)}", id);

            return data;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel, TIdType>.GetAllAsync()
        {
            var data = await _context.Set<TModel>()
                                .FromSqlRaw($"Get{nameof(TModel)}")
                                .ToListAsync();

            return data;
        }

        async Task<TModel> IGenericRepository<TModel, TIdType>.GetByIdAsync(TIdType id)
        {
            var data = await _context.Set<TModel>()
                                .FromSqlRaw($"Get{nameof(TModel)}ById {nameof(id)}", id)
                                .FirstOrDefaultAsync();

            return data;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.UpdateAsync(string[] paramNames, object[] paramValues)
        {
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();
            for (int i = 0; i < paramValues.Length; i++)
            {
                procedureParams.Add(new SqlParameter(paramNames[i], paramValues[i]));
                procedureParamNames.Append($"@{paramNames[i]},");
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var data = await _context.Database
                                .ExecuteSqlRawAsync($"Update{nameof(TModel)} {procedureParamNames}", procedureParams);

            return data;
        }
    }
}
