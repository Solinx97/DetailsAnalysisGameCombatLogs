using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class CommunityPostComment
{
    private CommunityPostComment()
    {
    }

    private CommunityPostComment(string content, int commentType, DateTimeOffset createdAt, string appUserId)
    {
        Content = content;
        CommentType = commentType;
        CreatedAt = createdAt;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Content { get; private set; }

    public int CommentType { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityPostId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostComment Create(string content, int commentType, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPostComment(content, commentType, createdAt, appUserId);
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
