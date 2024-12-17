using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

internal class SQLSPSpecScoreRepository : ISpecScore
{
    private readonly CombatParserSQLContext _context;

    public SQLSPSpecScoreRepository(CombatParserSQLContext context)
    {
        _context = context;
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
}
