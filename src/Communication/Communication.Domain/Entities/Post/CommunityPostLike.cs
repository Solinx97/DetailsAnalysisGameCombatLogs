using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Post;

public class CommunityPostLike
{
    private CommunityPostLike()
    {
    }

    private CommunityPostLike(CommunityId communityId, UserId appUserId, DateTimeOffset createdAt)
    {
        CommunityId = communityId;
        AppUserId = appUserId;
        CreatedAt = createdAt;
    }

    public CommunityPostLikeId Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public CommunityId CommunityId { get; private set; }

    public CommunityPostId CommunityPostId { get; private set; }

    public UserId AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostLike Create(CommunityId communityId, UserId appUserId)
    {
        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPostLike(communityId, appUserId, createdAt);
    }
}
