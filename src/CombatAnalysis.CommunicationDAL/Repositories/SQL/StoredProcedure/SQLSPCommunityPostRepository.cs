using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL.StoredProcedure;

internal class SQLSPCommunityPostRepository : SQLRepository<CommunityPost, int>, ICommunityPostRepository
{
    private readonly CommunicationSQLContext _context;

    public SQLSPCommunityPostRepository(CommunicationSQLContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommunityPost>> GetByCommunityIdAsync(int communityId, int pageSize)
    {
        var communityIdParam = new SqlParameter("CommunityId", communityId);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"Get{nameof(CommunityPost)}ByCommunityIdPagination @communityId, @pageSize",
                                            communityIdParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetMoreByCommunityIdAsync(int communityId, int offset, int pageSize)
    {
        var communityIdParam = new SqlParameter("CommunityId", communityId);
        var offsetParam = new SqlParameter("Offset", offset);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"GetMore{nameof(CommunityPost)}ByCommunityId @communityId, @offset, @pageSize",
                                            communityIdParam, offsetParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetNewByCommunityIdAsync(int communityId, DateTimeOffset checkFrom)
    {
        var communityIdParam = new SqlParameter("CommunityId", communityId);
        var checkFromParam = new SqlParameter("CheckFrom", checkFrom);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"GetNew{nameof(CommunityPost)}ByCommunityId @communityId, @checkFrom",
                                            communityIdParam, checkFromParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetByListOfCommunityIdAsync(string communityIds, int pageSize)
    {
        var communityIdsParam = new SqlParameter("CommunityIds", communityIds);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"Get{nameof(CommunityPost)}ByListOfCommunityIdPagination @communityIds, @pageSize",
                                            communityIdsParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetMoreByListOfCommunityIdAsync(string communityIds, int offset, int pageSize)
    {
        var communityIdsParam = new SqlParameter("CommunityIds", communityIds);
        var offsetParam = new SqlParameter("Offset", offset);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"GetMore{nameof(CommunityPost)}ByListOfCommunityId @communityIds, @offset, @pageSize",
                                            communityIdsParam, offsetParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetNewByListOfCommunityIdAsync(string communityIds, DateTimeOffset checkFrom)
    {
        var communityIdsParam = new SqlParameter("CommunityIds", communityIds);
        var checkFromParam = new SqlParameter("CheckFrom", checkFrom);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"GetNew{nameof(CommunityPost)}ByListOfCommunityId @communityIds, @checkFrom",
                                            communityIdsParam, checkFromParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<int> CountByCommunityIdAsync(int communityId)
    {
        var count = await _context.Set<CommunityPost>()
                     .CountAsync(cl => cl.CommunityId == communityId);

        return count;
    }

    public async Task<int> CountByListOfCommunityIdAsync(int[] communityIds)
    {
        var count = await _context.Set<CommunityPost>()
                     .CountAsync(cl => communityIds.Contains(cl.CommunityId));

        return count;
    }
}
