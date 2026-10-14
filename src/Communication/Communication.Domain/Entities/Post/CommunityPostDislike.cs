using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Post;

public class CommunityPostDislike
{
    private CommunityPostDislike()
    {
    }

    private CommunityPostDislike(CommunityId communityId, UserId appUserId, DateTimeOffset createdAt)
    {
        CommunityId = communityId;
        AppUserId = appUserId;
        CreatedAt = createdAt;
    }

    public CommunityPostDislikeId Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public CommunityId CommunityId { get; private set; }

    public CommunityPostId CommunityPostId { get; private set; }

    public UserId AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostDislike Create(CommunityId communityId, UserId appUserId)
    {
        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPostDislike(communityId, appUserId, createdAt);
    }
}
