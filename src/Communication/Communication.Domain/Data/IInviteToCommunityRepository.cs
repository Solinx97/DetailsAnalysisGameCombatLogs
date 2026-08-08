using Communication.Domain.Entities.Community;

namespace Communication.Domain.Data;

public interface IInviteToCommunityRepository
{
    Task<IEnumerable<InviteToCommunity>> GetByUserIdAsync(string appUserId, CancellationToken cancellationToken);
}
