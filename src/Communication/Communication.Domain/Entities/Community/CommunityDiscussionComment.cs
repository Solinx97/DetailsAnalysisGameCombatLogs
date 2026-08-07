using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Community;

public class CommunityDiscussionComment
{
    private CommunityDiscussionComment()
    {
    }

    private CommunityDiscussionComment(int id, string content, DateTimeOffset createdAt,
        int communityDiscussionId, string appUserId)
    {
        Id = id;
        Content = content;
        CreatedAt = createdAt;
        CommunityDiscussionId = communityDiscussionId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityDiscussionId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityDiscussion CommunityDiscussion { get; private set; }

    public static CommunityDiscussionComment Create(int id, string content, DateTimeOffset createdAt,
        int communityDiscussionId, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityDiscussionId, nameof(communityDiscussionId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new CommunityDiscussionComment(id, content, createdAt, communityDiscussionId, appUserId);
    }
}
