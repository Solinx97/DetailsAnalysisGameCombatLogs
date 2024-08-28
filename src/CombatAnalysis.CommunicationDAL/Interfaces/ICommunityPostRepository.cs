using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationDAL.Interfaces;

public interface ICommunityPostRepository : IGenericRepository<CommunityPost, int>
{
    Task<IEnumerable<CommunityPost>> GetByCommunityIdAsync(int communityId, int pageSize);

    Task<IEnumerable<CommunityPost>> GetMoreByCommunityIdAsync(int communityId, int offset, int pageSize);

    Task<IEnumerable<CommunityPost>> GetNewByCommunityIdAsync(int communityId, DateTimeOffset checkFrom);

    Task<int> CountByCommunityIdAsync(int communityId);
}
