using Communication.Domain.Entities.Community;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Data;

public interface IInviteToCommunityRepository
{
    Task<IEnumerable<InviteToCommunity>> GetByUserIdAsync(UserId appUserId, CancellationToken cancellationToken);
}
