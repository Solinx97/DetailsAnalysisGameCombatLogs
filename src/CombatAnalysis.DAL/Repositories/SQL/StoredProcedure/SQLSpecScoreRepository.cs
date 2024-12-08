using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

internal class SQLSpecScoreRepository : ISpecScore
{
    private readonly CombatParserSQLContext _context;

    public SQLSpecScoreRepository(CombatParserSQLContext context)
    {
        _context = context;
    }

    public async Task<SpecializationScore> CreateAsync(SpecializationScore item)
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

        var data = await Task.Run(() => _context.Set<SpecializationScore>().FromSqlRaw($"InsertInto{item.GetType().Name} {procedureParamNames}", procedureParams.ToArray())
                                            .AsEnumerable()
                                            .FirstOrDefault());

        return data;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _context.Database
                            .ExecuteSqlRawAsync($"Delete{typeof(SpecializationScore).Name}ById @Id", new SqlParameter("Id", id));

        return rowsAffected;
    }

    public async Task<IEnumerable<SpecializationScore>> GetAllAsync()
    {
        var data = await _context.Set<SpecializationScore>()
                            .FromSqlRaw($"GetAll{typeof(SpecializationScore).Name}")
                            .ToListAsync();

        return data;
    }

    public async Task<SpecializationScore> GetByIdAsync(int id)
    {
        var data = await Task.Run(() => _context.Set<SpecializationScore>()
                            .FromSqlRaw($"Get{typeof(SpecializationScore).Name}ById @Id", new SqlParameter("Id", id))
                            .AsEnumerable()
                            .FirstOrDefault());

        return data;
    }

    public async Task<IEnumerable<SpecializationScore>> GetBySpecIdAsync(int specId, int bossId, int difficult)
    {
        var sqlParameters = new SqlParameter[]
        {
            new SqlParameter(nameof(SpecializationScore.SpecId), specId),
            new SqlParameter(nameof(SpecializationScore.BossId), bossId),
            new SqlParameter(nameof(SpecializationScore.Difficult), difficult),
        };
        var data = await _context.Set<SpecializationScore>()
                            .FromSqlRaw($"Get{typeof(SpecializationScore).Name}BySpecId @{nameof(SpecializationScore.SpecId)}, @{nameof(SpecializationScore.BossId)}, @{nameof(SpecializationScore.Difficult)}", sqlParameters)
                            .ToListAsync();

        return data;
    }

    public IEnumerable<SpecializationScore> GetByParam(string paramName, object value)
    {
        var result = new List<SpecializationScore>();
        var data = _context.Set<SpecializationScore>()
                            .FromSqlRaw($"GetAll{typeof(SpecializationScore).Name}")
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

    public async Task<int> UpdateAsync(SpecializationScore item)
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
