using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityRepository(CommunicationContext context) : ICommunityRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<IEnumerable<CommunityUser>> GetCommunityUsersAsync(int id, CancellationToken cancellationToken)
    {
        var community = await _context.Set<Community>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityUsers)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community.CommunityUsers;
    }

    public async Task<IEnumerable<InviteToCommunity>> GetInvitesToCommunityAsync(int id, CancellationToken cancellationToken)
    {
        var community = await _context.Set<Community>()
            .Where(x => x.Id == id)
            .Include(x => x.InvitesToCommunity)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community.InvitesToCommunity;
    }

    public async Task<Community> GetWithCommunityUsersAsync(int id, CancellationToken cancellationToken)
    {
        var community = await _context.Set<Community>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityUsers)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community;
    }

    public async Task<Community> GetWithInvitesToCommunityAsync(int id, CancellationToken cancellationToken)
    {
        var community = await _context.Set<Community>()
            .Where(x => x.Id == id)
            .Include(x => x.InvitesToCommunity)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community;
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken)
    {
        var count = await _context.Set<Community>().CountAsync(cancellationToken);

        return count;
    }

    public async Task DeleteAsync(int id, CancellationToken cancelationToken)
    {
        await _context.Set<Community>()
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
