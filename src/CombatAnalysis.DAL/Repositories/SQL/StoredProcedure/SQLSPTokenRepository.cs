using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities.Authentication;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

public class SQLSPTokenRepository : ITokenRepository
{
    private readonly SQLContext _context;

    public SQLSPTokenRepository(SQLContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> CreateAsync(RefreshToken item)
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

        var data = await Task.Run(() => _context.Set<RefreshToken>().FromSqlRaw($"InsertInto{item.GetType().Name} {procedureParamNames}", procedureParams.ToArray())
                                            .AsEnumerable()
                                            .FirstOrDefault());

        return data;
    }

    public async Task<int> DeleteAsync(RefreshToken item)
    {
        var property = item.GetType().GetProperty("Id");
        var rowsAffected = await _context.Database
                            .ExecuteSqlRawAsync($"Delete{item.GetType().Name}ById {property.Name}", property.GetValue(item));

        return rowsAffected;
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        var result = new RefreshToken();
        var data = await _context.Set<RefreshToken>()
                            .FromSqlRaw($"GetAll{nameof(RefreshToken)}")
                            .ToListAsync();

        foreach (var item in data)
        {
            if (item.GetType().GetProperty(nameof(RefreshToken.Token)).GetValue(item).Equals(token))
            {
                result = item;
                break;
            }
        }

        return result;
    }

    public async Task<IEnumerable<RefreshToken>> GetAllByUserAsync(string userId)
    {
        var result = new List<RefreshToken>();
        var data = await _context.Set<RefreshToken>()
                            .FromSqlRaw($"GetAll{nameof(RefreshToken)}")
                            .ToListAsync();

        foreach (var item in data)
        {
            if (item.GetType().GetProperty(nameof(RefreshToken.UserId)).GetValue(item).Equals(userId))
            {
                result.Add(item);
            }
        }

        return result;
    }

    public async Task<int> UpdateAsync(RefreshToken item)
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
