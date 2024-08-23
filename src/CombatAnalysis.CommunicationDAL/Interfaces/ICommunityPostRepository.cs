using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationDAL.Interfaces;

public interface ICommunityPostRepository : IGenericRepository<CommunityPost, int>
{
    Task<IEnumerable<CommunityPost>> GetByCommunityIdAsyn(int communityId, int pageSize);

    Task<IEnumerable<CommunityPost>> GetMoreByCommunityIdAsyn(int communityId, int offset, int pageSize);

    Task<int> CountByCommunityIdAsync(int communityId);
}
