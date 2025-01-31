using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Interfaces.Generic;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

internal class SQLCountRepository<TModel> : ICountRepository<TModel>
    where TModel : class, ICombatPlayerEntity
{
    private readonly CombatParserSQLContext _context;

    public SQLCountRepository(CombatParserSQLContext context)
    {
        _context = context;
    }

    public async Task<int> CountByCombatPlayerIdAsync(int combatPlayerId)
    {
        var count = await _context.Set<TModel>()
                     .CountAsync(x => x.CombatPlayerId == combatPlayerId);

        return count;
    }
}
