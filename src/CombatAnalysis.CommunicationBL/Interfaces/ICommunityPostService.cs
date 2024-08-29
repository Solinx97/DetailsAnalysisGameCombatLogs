using CombatAnalysis.CommunicationBL.DTO.Post;

namespace CombatAnalysis.CommunicationBL.Interfaces;

public interface ICommunityPostService : IService<CommunityPostDto, int>
{
    Task<IEnumerable<CommunityPostDto>> GetByCommunityIdAsync(int communityId, int pageSize);

    Task<IEnumerable<CommunityPostDto>> GetMoreByCommunityIdAsync(int communityId, int offset, int pageSize);

    Task<IEnumerable<CommunityPostDto>> GetNewByCommunityIdAsync(int communityId, DateTimeOffset checkFrom);

    Task<IEnumerable<CommunityPostDto>> GetByListOfCommunityIdAsync(string communityIds, int pageSize);

    Task<IEnumerable<CommunityPostDto>> GetMoreByListOfCommunityIdAsync(string communityIds, int offset, int pageSize);

    Task<IEnumerable<CommunityPostDto>> GetNewByListOfCommunityIdAsync(string communityIds, DateTimeOffset checkFrom);

    Task<int> CountByCommunityIdAsync(int communityId);

    Task<int> CountByListOfCommunityIdAsync(int[] communityIds);
}
