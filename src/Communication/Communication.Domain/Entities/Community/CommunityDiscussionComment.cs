using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Community;

public class CommunityDiscussionComment
{
    private CommunityDiscussionComment()
    {
    }

    private CommunityDiscussionComment(string content, DateTimeOffset createdAt, string appUserId)
    {
        Content = content;
        CreatedAt = createdAt;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityDiscussionId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityDiscussion CommunityDiscussion { get; private set; }

    public static CommunityDiscussionComment Create(string content, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityDiscussionComment(content, createdAt, appUserId);
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
