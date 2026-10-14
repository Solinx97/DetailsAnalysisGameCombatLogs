using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface ICommunityRepository
{
    Task<(IEnumerable<Community>, int)> GetAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task<(IEnumerable<Community>, int)> GetByUserIdAsync(UserId appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersAsync(CommunityId communityId, int page, int pageSize, CancellationToken cancellationToken);

    Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersByUserIdAsync(UserId appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<bool> CanJoinAsync(UserId appUserId, CommunityId communityId, CancellationToken cancellationToken);

    Task<Community> GetWithUsersAsync(CommunityId id, CancellationToken cancellationToken);

    Task<Community> GetWithInvitesAsync(CommunityId id, CancellationToken cancellationToken);

    Task<Community> GetWithInvitesAndUsersAsync(CommunityId id, CancellationToken cancellationToken);

    Task DeleteAsync(CommunityId id, CancellationToken cancelationToken);
}
