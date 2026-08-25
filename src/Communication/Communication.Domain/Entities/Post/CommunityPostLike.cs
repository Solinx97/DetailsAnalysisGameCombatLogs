using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class CommunityPostLike
{
    private CommunityPostLike()
    {
    }

    private CommunityPostLike(DateTimeOffset createdAt, int communityId, string appUserId)
    {
        CreatedAt = createdAt;
        CommunityId = communityId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityId { get; private set; }

    public int CommunityPostId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostLike Create(int communityId, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPostLike(createdAt, communityId, appUserId);
    }
}
