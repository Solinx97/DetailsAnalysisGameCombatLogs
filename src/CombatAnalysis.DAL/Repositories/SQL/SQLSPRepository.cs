using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatAnalysis.DAL.Repositories.SQL
{
    public class SQLSPRepository<TModel, TIdType> : IGenericRepository<TModel, TIdType>
        where TModel : class
        where TIdType : notnull
    {
        private readonly SQLContext _context;

        public SQLSPRepository(SQLContext context)
        {
            _context = context;
        }

        async Task<TModel> IGenericRepository<TModel, TIdType>.CreateAsync(TModel item)
        {
            var properties = item.GetType().GetProperties();
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();

            for (int i = 1; i < properties.Length; i++)
            {
                if (properties[i].CanWrite)
                {
                    procedureParams.Add(new SqlParameter(properties[i].Name, properties[i].GetValue(item)));
                    procedureParamNames.Append($"@{properties[i].Name},");
                }
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var res = await Task.Run(() => _context.Set<TModel>().FromSqlRaw($"InsertInto{item.GetType().Name} {procedureParamNames}", procedureParams.ToArray())
                                                .AsEnumerable().FirstOrDefault());

            return res;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.DeleteAsync(TModel item)
        {
            var property = item.GetType().GetProperty("Id");
            var data = await _context.Database
                                .ExecuteSqlRawAsync($"Delete{item.GetType().Name}ById {property.Name}", property.GetValue(item));

            return data;
        }

        async Task<IEnumerable<TModel>> IGenericRepository<TModel, TIdType>.GetAllAsync()
        {
            var data = await _context.Set<TModel>()
                                .FromSqlRaw($"GetAll{typeof(TModel).Name}")
                                .ToListAsync();

            return data;
        }

        async Task<TModel> IGenericRepository<TModel, TIdType>.GetByIdAsync(TIdType id)
        {
            var data = await Task.Run(() => _context.Set<TModel>()
                                .FromSqlRaw($"Get{typeof(TModel).Name}ById @Id", new SqlParameter("Id", id))
                                .AsEnumerable()
                                .FirstOrDefault());

            return data;
        }

        public IEnumerable<TModel> GetByParam(string paramName, object value)
        {
            var result = new List<TModel>();
            var data = _context.Set<TModel>()
                                .FromSqlRaw($"GetAll{typeof(TModel).Name}");
            foreach (var item in data)
            {
                if (item.GetType().GetProperty(paramName).Equals(value))
                {
                    result.Add(item);
                }
            }

            return result;
        }

        async Task<int> IGenericRepository<TModel, TIdType>.UpdateAsync(TModel item)
        {
            var properties = item.GetType().GetProperties();
            var procedureParams = new List<SqlParameter>();
            var procedureParamNames = new StringBuilder();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].CanWrite)
                {
                    procedureParams.Add(new SqlParameter(properties[i].Name, properties[i].GetValue(item)));
                    procedureParamNames.Append($"@{properties[i].Name},");
                }
            }
            procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

            var data = await _context.Database
                                .ExecuteSqlRawAsync($"Update{item.GetType().Name} {procedureParamNames}", procedureParams);

            return data;
        }
    }
}
