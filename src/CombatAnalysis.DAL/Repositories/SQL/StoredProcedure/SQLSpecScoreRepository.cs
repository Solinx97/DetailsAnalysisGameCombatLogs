using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

public class SQLSpecScoreRepository<TModel, TIdType> : ISpecScore<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    private readonly CombatParserSQLContext _context;

    public SQLSpecScoreRepository(CombatParserSQLContext context)
    {
        _context = context;
    }

    public async Task<TModel> CreateAsync(TModel item)
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

        var data = await Task.Run(() => _context.Set<TModel>().FromSqlRaw($"InsertInto{item.GetType().Name} {procedureParamNames}", procedureParams.ToArray())
                                            .AsEnumerable()
                                            .FirstOrDefault());

        return data;
    }

    public async Task<int> DeleteAsync(TIdType id)
    {
        var rowsAffected = await _context.Database
                            .ExecuteSqlRawAsync($"Delete{typeof(TModel).Name}ById @Id", new SqlParameter("Id", id));

        return rowsAffected;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        var data = await _context.Set<TModel>()
                            .FromSqlRaw($"GetAll{typeof(TModel).Name}")
                            .ToListAsync();

        return data;
    }

    public async Task<TModel> GetByIdAsync(TIdType id)
    {
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSqlRaw($"Get{typeof(TModel).Name}ById @Id", new SqlParameter("Id", id))
                            .AsEnumerable()
                            .FirstOrDefault());

        return data;
    }

    public async Task<IEnumerable<TModel>> GetBySpecIdAsync(int specId, int bossId, int difficult)
    {
        var sqlParameters = new SqlParameter[]
        {
            new SqlParameter("SpecId", specId),
            new SqlParameter("BossId", bossId),
            new SqlParameter("Difficult", difficult),
        };
        var data = await _context.Set<TModel>()
                            .FromSqlRaw($"Get{typeof(TModel).Name}BySpecId @SpecId, @BossId, @Difficult", sqlParameters)
                            .ToListAsync();

        return data;
    }

    public IEnumerable<TModel> GetByParam(string paramName, object value)
    {
        var result = new List<TModel>();
        var data = _context.Set<TModel>()
                            .FromSqlRaw($"GetAll{typeof(TModel).Name}")
                            .AsEnumerable();
        foreach (var item in data)
        {
            if (item.GetType().GetProperty(paramName).GetValue(item).Equals(value))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public async Task<int> UpdateAsync(TModel item)
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

        var rowsAffected = await _context.Database
                            .ExecuteSqlRawAsync($"Update{item.GetType().Name} {procedureParamNames}", procedureParams);

        return rowsAffected;
    }
}
