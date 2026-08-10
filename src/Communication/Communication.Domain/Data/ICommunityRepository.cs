using Communication.Domain.Aggregates;
using Communication.Domain.Entities.Community;

namespace Communication.Domain.Data;

public interface ICommunityRepository
{
    Task<IEnumerable<Community>> GetByUserIdAsync(string appUserId, CancellationToken cancellationToken);

    Task<IEnumerable<CommunityUser>> GetCommunityUsersAsync(int id, CancellationToken cancellationToken);

    Task<IEnumerable<InviteToCommunity>> GetInvitesToCommunityAsync(int id, CancellationToken cancellationToken);

    Task<Community> GetWithCommunityUsersAsync(int id, CancellationToken cancellationToken);

    Task<Community> GetWithInvitesToCommunityAsync(int id, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancelationToken);

    Task<int> CountAsync(CancellationToken cancellationToken);
}
