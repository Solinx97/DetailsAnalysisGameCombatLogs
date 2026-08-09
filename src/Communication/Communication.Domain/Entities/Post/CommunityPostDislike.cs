using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class CommunityPostDislike
{
    private CommunityPostDislike()
    {
    }

    private CommunityPostDislike(DateTimeOffset createdAt, string appUserId)
    {
        CreatedAt = createdAt;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityPostId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostDislike Create(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPostDislike(createdAt, appUserId);
    }
}
