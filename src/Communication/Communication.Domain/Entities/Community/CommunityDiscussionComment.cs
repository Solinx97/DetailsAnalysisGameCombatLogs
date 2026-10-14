using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Community;

public class CommunityDiscussionComment
{
    public const int CONTENT_MAX_LENGTH = 256;

    private CommunityDiscussionComment()
    {
    }

    private CommunityDiscussionComment(string content, UserId appUserId, DateTimeOffset createdAt)
    {
        CreatedAt = createdAt;
        Content = content;
        AppUserId = appUserId;
    }

    public CommunityDiscussionCommentId Id { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public CommunityDiscussionId CommunityDiscussionId { get; private set; }

    public UserId AppUserId { get; private set; }

    public CommunityDiscussion CommunityDiscussion { get; private set; }

    public static CommunityDiscussionComment Create(string content, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityDiscussionComment(content, appUserId, createdAt);
    }

    public void EditContent(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        if (!string.Equals(Content, content, StringComparison.CurrentCultureIgnoreCase))
        {
            Content = content;
        }
    }
}
