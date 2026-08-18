using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class CommunityPostComment
{
    public const int CONTENT_MAX_LENGTH = 256;

    private CommunityPostComment()
    {
    }

    private CommunityPostComment(string content, int commentType, DateTimeOffset createdAt, int communityId, string appUserId)
    {
        Content = content;
        CommentType = commentType;
        CreatedAt = createdAt;
        CommunityId = communityId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Content { get; private set; }

    public int CommentType { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityId { get; private set; }

    public int CommunityPostId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostComment Create(string content, int commentType, int communityId, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPostComment(content, commentType, createdAt, communityId, appUserId);
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
