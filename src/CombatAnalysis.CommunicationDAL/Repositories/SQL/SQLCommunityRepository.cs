using CombatAnalysis.CommunicationDAL.Data;
using CombatAnalysis.CommunicationDAL.Entities.Community;
using CombatAnalysis.CommunicationDAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CombatAnalysis.CommunicationDAL.Repositories.SQL;

public class SQLCommunityRepository : SQLRepository<Community, int>, ICommunityRepository
{
    private readonly SQLContext _context;

    public SQLCommunityRepository(SQLContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Community>> GetAllWithPaginationAsync(int pageSize)
    {
        var result = await _context.Set<Community>()
            .OrderBy(c => c.Id)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    public async Task<IEnumerable<Community>> GetMoreWithPaginationAsync(int offset, int pageSize)
    {
        var result = await _context.Set<Community>()
            .OrderBy(c => c.Id)
            .Skip(offset)
            .Take(pageSize)
            .ToListAsync();

        return result;
    }

    public async Task<int> CountAsync()
    {
        var count = await _context.Set<Community>()
             .CountAsync();

        return count;
    }
}
