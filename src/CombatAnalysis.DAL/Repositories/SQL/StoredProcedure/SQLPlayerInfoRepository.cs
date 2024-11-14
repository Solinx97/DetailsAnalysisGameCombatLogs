using CombatAnalysis.DAL.Data;
using CombatAnalysis.DAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.DAL.Repositories.SQL.StoredProcedure;

internal class SQLPlayerInfoRepository<TModel, TIdType> : SQLSPRepository<TModel, TIdType>, IPlayerInfo<TModel, TIdType>
    where TModel : class
    where TIdType : notnull
{
    private readonly CombatParserSQLContext _context;

    public SQLPlayerInfoRepository(CombatParserSQLContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId)
    {
        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSqlRaw($"Get{typeof(TModel).Name}ByCombatPlayerId @CombatPlayerId", new SqlParameter("CombatPlayerId", combatPlayerId))
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<TModel>> GetByCombatPlayerIdAsync(int combatPlayerId, int page, int pageSize)
    {
        var combatPlayerIdParam = new SqlParameter("CombatPlayerId", combatPlayerId);
        var pageParam = new SqlParameter("Page", page);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<TModel>()
                            .FromSqlRaw($"Get{typeof(TModel).Name}ByCombatPlayerIdPagination @combatPlayerId, @page, @pageSize",
                                            combatPlayerIdParam, pageParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }
}