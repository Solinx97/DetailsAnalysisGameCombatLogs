using CombatAnalysis.CommunicationDAL.Entities.Post;

namespace CombatAnalysis.CommunicationDAL.Interfaces;

public interface IUserPostRepository : IGenericRepository<UserPost, int>
{
    Task<IEnumerable<UserPost>> GetByAppUserIdAsync(string appUserId, int pageSize);

    Task<IEnumerable<UserPost>> GetMoreByAppUserIdAsync(string appUserId, int offset, int pageSize);

    Task<IEnumerable<UserPost>> GetNewByAppUserIdAsync(string appUserId, DateTimeOffset checkFrom);

    Task<int> CountByAppUserIdAsync(string appUserId);
}