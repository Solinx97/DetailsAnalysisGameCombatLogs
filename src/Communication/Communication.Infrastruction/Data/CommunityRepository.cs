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

    public async Task<(IEnumerable<Community>, int)> GetByUserIdAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Set<CommunityUser>()
            .AsNoTracking()
            .Where(x => x.AppUserId == appUserId);

        var communities = await query
            .Select(x => x.Community)
            .Distinct()
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await query
            .CountAsync(cancellationToken);

        return (communities, count);
    }

    public async Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersAsync(int communityId, int page, int pageSize,  CancellationToken cancellationToken)
    {
        var query = _context.Set<CommunityUser>()
            .AsNoTracking()
            .Where(x => x.CommunityId == communityId);

        var communityUsers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await query
            .CountAsync(cancellationToken);

        return (communityUsers, count);
    }
    public async Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersByUserIdAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Set<CommunityUser>()
            .AsNoTracking()
            .Where(x => x.AppUserId == appUserId);

        var communityUsers = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var count = await query
            .CountAsync(cancellationToken);

        return (communityUsers, count);
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

    public async Task<Community> GetWithUsersAsync(int id, CancellationToken cancellationToken)
    {
        var community = await _context.Set<Community>()
            .Where(x => x.Id == id)
            .Include(x => x.CommunityUsers)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community;
    }

    public async Task<Community> GetWithInvitesAsync(int id, CancellationToken cancellationToken)
    {
        var community = await _context.Set<Community>()
            .Where(x => x.Id == id)
            .Include(x => x.InvitesToCommunity)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community;
    }

    public async Task<Community> GetWithInvitesAndUsersAsync(int id, CancellationToken cancellationToken)
    {
        var community = await _context.Set<Community>()
            .Where(x => x.Id == id)
            .Include(x => x.InvitesToCommunity)
            .Include(x => x.CommunityUsers)
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
