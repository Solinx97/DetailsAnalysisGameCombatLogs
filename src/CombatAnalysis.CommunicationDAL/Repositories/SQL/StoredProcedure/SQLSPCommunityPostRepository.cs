using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL.StoredProcedure;

public class SQLSPCommunityPostRepository : SQLRepository<CommunityPost, int>, ICommunityPostRepository
{
    private readonly SQLContext _context;

    public SQLSPCommunityPostRepository(SQLContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommunityPost>> GetByCommunityIdAsyn(int communityId, int pageSize)
    {
        var communityIdParam = new SqlParameter("CommunityId", communityId);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"Get{nameof(CommunityPost)}ByCommunityIdPagination @communityId, @pageSize",
                                            communityIdParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<CommunityPost>> GetMoreByCommunityIdAsyn(int communityId, int offset, int pageSize)
    {
        var communityIdParam = new SqlParameter("CommunityId", communityId);
        var offsetParam = new SqlParameter("Offset", offset);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<CommunityPost>()
                            .FromSqlRaw($"Get{nameof(CommunityPost)}ByCommunityIdMore @communityId, @offset, @pageSize",
                                            communityIdParam, offsetParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<int> CountByCommunityIdAsync(int communityId)
    {
        var count = await _context.Set<CommunityPost>()
                     .CountAsync(cl => cl.CommunityId == communityId);

        return count;
    }
}
