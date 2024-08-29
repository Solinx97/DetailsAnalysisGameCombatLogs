using CombatAnalysis.CommunicationBL.DTO.Post;

namespace CombatAnalysis.CommunicationBL.Interfaces;

public interface IUserPostService : IService<UserPostDto, int>
{
    Task<IEnumerable<UserPostDto>> GetByAppUserIdAsync(string appUserId, int pageSize);

    Task<IEnumerable<UserPostDto>> GetMoreByAppUserIdAsync(string appUserId, int offset, int pageSize);

    Task<IEnumerable<UserPostDto>> GetNewByAppUserIdAsync(string appUserId, DateTimeOffset checkFrom);

    Task<IEnumerable<UserPostDto>> GetByListOfAppUserIdAsync(string appUserIds, int pageSize);

    Task<IEnumerable<UserPostDto>> GetMoreByListOfAppUserIdAsync(string appUserIds, int offset, int pageSize);

    Task<IEnumerable<UserPostDto>> GetNewByListOfAppUserIdAsync(string appUserIds, DateTimeOffset checkFrom);

    Task<int> CountByAppUserIdAsync(string appUserId);

    Task<int> CountByListOfAppUserIdAsync(string[] appUserIds);
}
