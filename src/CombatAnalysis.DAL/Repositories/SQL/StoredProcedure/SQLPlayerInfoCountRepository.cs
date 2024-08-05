using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Entities;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

public class SQLPlayerInfoCountRepository<TModel, TIdType> : SQLPlayerInfoRepository<TModel, TIdType>, IPlayerInfoCount<TModel, TIdType>
    where TModel : BasePlayerInfo
    where TIdType : notnull
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
