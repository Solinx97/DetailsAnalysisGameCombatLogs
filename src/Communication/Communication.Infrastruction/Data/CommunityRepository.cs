using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.Entities.Community;
using Communication.Domain.ValueObjects;
using Communication.Infrastruction.Exceptions;
using Communication.Infrastruction.Persistent;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastruction.Data;

internal class CommunityRepository(CommunicationContext context) : ICommunityRepository
{
    private readonly CommunicationContext _context = context;

    public async Task<(IEnumerable<Community>, int)> GetAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CommunityUser
            .AsNoTracking();

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

    public async Task<(IEnumerable<Community>, int)> GetByUserIdAsync(UserId appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CommunityUser
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

    public async Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersAsync(CommunityId communityId, int page, int pageSize,  CancellationToken cancellationToken)
    {
        var query = _context.CommunityUser
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

    public async Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersByUserIdAsync(UserId appUserId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CommunityUser
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

    public async Task<bool> CanJoinAsync(UserId appUserId, CommunityId communityId, CancellationToken cancellationToken)
    {
        var count = await _context.CommunityUser
            .AsNoTracking()
            .Where(x => x.AppUserId == appUserId && x.CommunityId == communityId)
            .CountAsync(cancellationToken);

        return count == 0;
    }

    public async Task<Community> GetWithUsersAsync(CommunityId id, CancellationToken cancellationToken)
    {
        var community = await _context.Community
            .Where(x => x.Id == id)
            .Include(x => x.CommunityUsers)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community;
    }

    public async Task<Community> GetWithInvitesAsync(CommunityId id, CancellationToken cancellationToken)
    {
        var community = await _context.Community
            .Where(x => x.Id == id)
            .Include(x => x.InvitesToCommunity)
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community;
    }

    public async Task<Community> GetWithInvitesAndUsersAsync(CommunityId id, CancellationToken cancellationToken)
    {
        var community = await _context.Community
            .Where(x => x.Id == id)
            .Include(x => x.InvitesToCommunity)
            .Include(x => x.CommunityUsers)
            .AsSplitQuery()
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new EntityNotFoundException(typeof(Community), id);

        return community;
    }

    public async Task DeleteAsync(CommunityId id, CancellationToken cancelationToken)
    {
        await _context.Community
            .Where(cl => cl.Id == id)
            .ExecuteDeleteAsync(cancelationToken);
    }
}
