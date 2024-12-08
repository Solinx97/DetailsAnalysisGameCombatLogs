using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using CombatAnalysis.DAL.Interfaces.Entities;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

internal class SQLPlayerInfoCountRepository<TModel> : SQLPlayerInfoRepository<TModel>, IPlayerInfoCount<TModel>
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
                     .CountAsync(cl => cl.CombatPlayerId == combatPlayerId);

        return count;
    }
}
