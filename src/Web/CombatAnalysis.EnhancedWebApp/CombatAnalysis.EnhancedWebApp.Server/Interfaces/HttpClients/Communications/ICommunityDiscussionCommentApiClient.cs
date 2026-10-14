using CombatAnalysis.EnhancedWebApp.Server.Models.Community;
using CombatAnalysis.EnhancedWebApp.Server.Patches;
using CombatAnalysis.EnhancedWebApp.Server.Response;

namespace CombatAnalysis.EnhancedWebApp.Server.Interfaces.HttpClients.Communications;

public interface ICommunityDiscussionCommentApiClient
{
    Task<DiscussionCommentResponse> GetAsync(int communityId, int page, int pageSize, CancellationToken cancellationToken);

    Task<CommunityDiscussionCommentModel> CreateAsync(CommunityDiscussionCommentModel discussion, CancellationToken cancellationToken);

    Task UpdateAsync(int id, CommunityDiscussionCommentPatch discussion, CancellationToken cancellationToken);

    Task DeleteAsync(int id, int discussionId, CancellationToken cancellationToken);
}
