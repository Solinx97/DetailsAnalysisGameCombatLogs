using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityDiscussionRepository(CommunicationContext context) : ICommunityDiscussionRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<CommunityDiscussion>> GetAsync(int communityId, int page, int pageSize, CancellationToken cancelationToken)
    {
        var result = await _context.Set<CommunityDiscussion>()
            .AsNoTracking()
            .Where(x => x.CommunityId == communityId)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancelationToken);
        return result.Count != 0 ? result : [];
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<CommunityDiscussion>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
