using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class InviteToCommunityRepository(CommunicationContext context) : IInviteToCommunityRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<InviteToCommunity>> GetByUserIdAsync(string appUserId, CancellationToken cancellationToken)
    {
        var invitesToCommunity = await _context.Set<InviteToCommunity>()
            .Where(x => x.ToAppUserId == appUserId)
            .ToListAsync(cancellationToken);

        return invitesToCommunity;
    }
}
