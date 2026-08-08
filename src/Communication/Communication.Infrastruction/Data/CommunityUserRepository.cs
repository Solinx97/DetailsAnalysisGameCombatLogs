using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityUserRepository(CommunicationContext context) : ICommunityUserRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<CommunityUser>> GetByCommunityIdAsync(int communityId, CancellationToken cancellationToken)
    {
        var communities = await _context.Set<CommunityUser>()
            .Where(x => x.CommunityId == communityId)
            .ToListAsync(cancellationToken);

        return communities.Count != 0 ? communities : [];
    }
}
