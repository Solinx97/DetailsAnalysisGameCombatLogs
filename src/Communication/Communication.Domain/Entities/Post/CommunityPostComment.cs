using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Post;

public class CommunityPostComment
{
    public const int CONTENT_MAX_LENGTH = 256;

    private CommunityPostComment()
    {
    }

    private CommunityPostComment(string content, int commentType, CommunityId communityId, UserId appUserId, DateTimeOffset createdAt)
    {
        Content = content;
        CommentType = commentType;
        CommunityId = communityId;
        AppUserId = appUserId;
        CreatedAt = createdAt;
    }

    public CommunityPostCommentId Id { get; private set; }

    public string Content { get; private set; }

    public int CommentType { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public CommunityId CommunityId { get; private set; }

    public CommunityPostId CommunityPostId { get; private set; }

    public UserId AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostComment Create(string content, int commentType, CommunityId communityId, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPostComment(content, commentType, communityId, appUserId, createdAt);
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
