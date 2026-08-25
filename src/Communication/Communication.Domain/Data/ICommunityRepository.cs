using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;

namespace Communication.Domain.Data;

public interface ICommunityRepository
{
    Task<(IEnumerable<Community>, int)> GetAsync(int page, int pageSize, CancellationToken cancellationToken);

    Task<(IEnumerable<Community>, int)> GetByUserIdAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken);

    Task<(IEnumerable<CommunityUser>, int)> GetCommunityUsersByUserIdAsync(string appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<bool> CanJoinAsync(string appUserId, int communityId, CancellationToken cancellationToken);

    Task<IEnumerable<InviteToCommunity>> GetInvitesToCommunityAsync(int id, CancellationToken cancellationToken);

    Task<Community> GetWithUsersAsync(int id, CancellationToken cancellationToken);

    Task<Community> GetWithInvitesAsync(int id, CancellationToken cancellationToken);

    Task<Community> GetWithInvitesAndUsersAsync(int id, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);

    Task<int> CountAsync(CancellationToken cancellationToken);
}
