using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Post;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL.StoredProcedure;

public class SQLSPUserPostRepository : SQLRepository<UserPost, int>, IUserPostRepository
{
    private readonly SQLContext _context;

    public SQLSPUserPostRepository(SQLContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserPost>> GetByAppUserIdAsync(string appUserId, int pageSize)
    {
        var apUserIdParam = new SqlParameter("AppUserId", appUserId);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSqlRaw($"Get{nameof(UserPost)}ByAppUserIdPagination @appUserId, @pageSize",
                                            apUserIdParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<UserPost>> GetMoreByAppUserIdAsync(string appUserId, int offset, int pageSize)
    {
        var apUserIdParam = new SqlParameter("AppUserId", appUserId);
        var offsetParam = new SqlParameter("Offset", offset);
        var pageSizeParam = new SqlParameter("PageSize", pageSize);

        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSqlRaw($"GetMore{nameof(UserPost)}ByAppUserId @appUserId, @offset, @pageSize",
                                            apUserIdParam, offsetParam, pageSizeParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<IEnumerable<UserPost>> GetNewByAppUserIdAsync(string appUserId, DateTimeOffset checkFrom)
    {
        var apUserIdParam = new SqlParameter("AppUserId", appUserId);
        var checkFromParam = new SqlParameter("CheckFrom", checkFrom);

        var data = await Task.Run(() => _context.Set<UserPost>()
                            .FromSqlRaw($"GetNew{nameof(UserPost)}ByAppUserId @appUserId, @checkFrom",
                                            apUserIdParam, checkFromParam)
                            .AsEnumerable());

        return data;
    }

    public async Task<int> CountByAppUserIdAsync(string appUserId)
    {
        var count = await _context.Set<UserPost>()
                     .CountAsync(cl => cl.AppUserId == appUserId);

        return count;
    }
}
