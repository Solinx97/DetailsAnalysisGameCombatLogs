using CombatAnalysis.CommunicationBL.DTO.Community;

namespace CombatAnalysis.CommunicationBL.Interfaces;

public interface ICommunityService : IService<CommunityDto, int>
{
    Task<IEnumerable<CommunityDto>> GetAllWithPaginationAsync(int pageSize);

    Task<IEnumerable<CommunityDto>> GetMoreWithPaginationAsync(int offset, int pageSize);

    Task<int> CountAsync();
}
