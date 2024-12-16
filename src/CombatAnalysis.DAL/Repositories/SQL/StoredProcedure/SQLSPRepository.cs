using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

internal class SQLSPRepository<TModel> : IGenericRepository<TModel>
    where TModel : class, IEntity
{
    private readonly CombatParserSQLContext _context;

    public SQLSPRepository(CombatParserSQLContext context)
    {
        _context = context;
    }

    public async Task<TModel> CreateAsync(TModel item)
    {
        var properties = item.GetType().GetProperties();
        var procedureParams = new List<SqlParameter>();
        var procedureParamNames = new StringBuilder();

        for (var i = 1; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParams.Add(new SqlParameter(properties[i].Name, properties[i].GetValue(item)));
                procedureParamNames.Append($"@{properties[i].Name},");
            }
        }

        procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

        var data = await Task.Run(() => _context.Set<TModel>().FromSqlRaw($"InsertInto{item.GetType().Name} {procedureParamNames}", procedureParams.ToArray())
                                            .AsEnumerable()
                                            .FirstOrDefault());

        return data;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _context.Database
                            .ExecuteSqlRawAsync($"Delete{typeof(TModel).Name}ById @Id", new SqlParameter("Id", id));

        return rowsAffected;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSqlRaw($"GetAll{typeof(TModel).Name}")
                            .AsEnumerable());

        return data;
    }

    public async Task<TModel> GetByIdAsync(int id)
    {
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSqlRaw($"Get{typeof(TModel).Name}ById @Id", new SqlParameter("Id", id))
                            .AsEnumerable()
                            .FirstOrDefault());

        return data;
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
        var properties = item.GetType().GetProperties();
        var procedureParams = new List<SqlParameter>();
        var procedureParamNames = new StringBuilder();
        for (var i = 0; i < properties.Length; i++)
        {
            if (properties[i].CanWrite)
            {
                procedureParams.Add(new SqlParameter(properties[i].Name, properties[i].GetValue(item)));
                procedureParamNames.Append($"@{properties[i].Name},");
            }
        }

        procedureParamNames.Remove(procedureParamNames.Length - 1, 1);

        var rowsAffected = await _context.Database
                            .ExecuteSqlRawAsync($"Update{item.GetType().Name} {procedureParamNames}", procedureParams);

        return rowsAffected;
    }
}
