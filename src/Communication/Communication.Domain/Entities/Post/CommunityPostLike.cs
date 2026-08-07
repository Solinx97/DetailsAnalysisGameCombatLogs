using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class CommunityPostLike
{
    private CommunityPostLike()
    {
    }

    private CommunityPostLike(int id, DateTimeOffset createdAt, int communityPostId, int communityId,
        string appUserId)
    {
        Id = id;
        CreatedAt = createdAt;
        CommunityPostId = communityPostId;
        CommunityId = communityId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityPostId { get; private set; }

    public int CommunityId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostLike Create(int id, DateTimeOffset createdAt, int communityPostId, int communityId,
        string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityPostId, nameof(communityPostId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new CommunityPostLike(id, createdAt, communityPostId, communityId, appUserId);
    }
}
