using CombatAnalysis.CommunicationDAL.Entities.Community;

namespace CombatAnalysis.CommunicationDAL.Interfaces;

public interface ICommunityRepository : IGenericRepository<Community, int>
{
    Task<IEnumerable<Community>> GetAllWithPaginationAsync(int pageSize);

    Task<IEnumerable<Community>> GetMoreWithPaginationAsync(int offset, int pageSize);

    Task<int> CountAsync();
}
