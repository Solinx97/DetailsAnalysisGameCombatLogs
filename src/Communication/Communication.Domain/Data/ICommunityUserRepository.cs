using Communication.Domain.Entities.Community;

namespace Communication.Domain.Data;

public interface ICommunityUserRepository
{
    Task<IEnumerable<CommunityUser>> GetByCommunityIdAsync(int communityId, CancellationToken cancellationToken);
}
