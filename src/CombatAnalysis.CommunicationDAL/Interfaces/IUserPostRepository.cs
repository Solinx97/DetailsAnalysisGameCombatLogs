using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationDAL.Interfaces;

public interface IUserPostRepository : IGenericRepository<UserPost, int>
{
    Task<IEnumerable<UserPost>> GetByAppUserIdAsyn(string appUserId, int pageSize);

    Task<IEnumerable<UserPost>> GetMoreByAppUserIdAsyn(string appUserId, int offset, int pageSize);

    Task<int> CountByAppUserIdAsync(string appUserId);
}