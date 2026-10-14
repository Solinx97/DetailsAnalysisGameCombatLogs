using CombatAnalysis.EnhancedWebApp.Server.Models.Post;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface IUserPostApiClient
{
    Task<UserPostsResponse> GetByUserIdAsync(Guid appUserId, int page, int pageSize, CancellationToken cancellationToken);

    Task<UserPostModel> CreateAsync(UserPostModel pos, CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);
}
