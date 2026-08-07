using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class CommunityPostComment
{
    private CommunityPostComment()
    {
    }

    private CommunityPostComment(int id, string content, int commentType, DateTimeOffset createdAt,
       int communityPostId, int communityId, string appUserId)
    {
        Id = id;
        Content = content;
        CommentType = commentType;
        CreatedAt = createdAt;
        CommunityPostId = communityPostId;
        CommunityId = communityId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Content { get; private set; }

    public int CommentType { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int CommunityPostId { get; private set; }

    public int CommunityId { get; private set; }

    public string AppUserId { get; private set; }

    public CommunityPost CommunityPost { get; private set; }

    public static CommunityPostComment Create(int id, string content, int commentType, DateTimeOffset createdAt,
       int communityPostId, int communityId, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityPostId, nameof(communityPostId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new CommunityPostComment(id, content, commentType, createdAt, communityPostId, communityId, appUserId);
    }
}
