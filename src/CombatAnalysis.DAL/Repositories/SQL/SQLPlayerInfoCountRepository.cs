using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL;

internal class SQLPlayerInfoCountRepository<TModel> : SQLSPPlayerInfoRepository<TModel>, IPlayerInfoCount<TModel>
    where TModel : class, ICombatPlayerEntity
{
    private readonly CombatParserSQLContext _context;

    public SQLPlayerInfoCountRepository(CombatParserSQLContext context) : base(context)
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
