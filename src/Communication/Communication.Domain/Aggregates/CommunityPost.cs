using Communication.Domain.Entities.Post;

namespace Communication.Domain.Aggregates;

public class CommunityPost
{
    public const int COMMUNITY_NAME_MAX_LENGTH = 128;

    private readonly List<CommunityPostComment> _communityPostComments = [];
    private readonly List<CommunityPostDislike> _communityPostDislikes = [];
    private readonly List<CommunityPostLike> _communityPostLikes = [];

    private CommunityPost()
    {
    }

    private CommunityPost(int id, string communityName, string owner, string content, 
        int postType, int publicType, int restrictions, string tags,
        DateTimeOffset createdAt, int likeCount, int dislikeCount, int commentCount,
        int communityId, string appUserId)
    {
        Id = id;
        CommunityName = communityName;
        Owner = owner;
        Content = content;
        PostType = postType;
        PublicType = publicType;
        Restrictions = restrictions;
        Tags = tags;
        CreatedAt = createdAt;
        LikeCount = likeCount;
        DislikeCount = dislikeCount;
        CommentCount = commentCount;
        CommunityId = communityId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string CommunityName { get; private set; }

    public string Owner { get; private set; }

    public string Content { get; private set; }

    public int PostType { get; private set; }

    public int PublicType { get; private set; }

    public int Restrictions { get; private set; }

    public string Tags { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int LikeCount { get; private set; }

    public int DislikeCount { get; private set; }

    public int CommentCount { get; private set; }

    public int CommunityId { get; private set; }

    public string AppUserId { get; private set; }

    public Community Community { get; private set; }

    public IReadOnlyList<CommunityPostComment> CommunityPostComments => _communityPostComments;

    public IReadOnlyList<CommunityPostDislike> CommunityPostDislikes => _communityPostDislikes;

    public IReadOnlyList<CommunityPostLike> CommunityPostLikes => _communityPostLikes;

    public static CommunityPost Create(int id, string communityName, string owner, string content,
        int postType, int publicType, int restrictions, string tags,
        DateTimeOffset createdAt, int likeCount, int dislikeCount, int commentCount,
        int communityId, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(owner, nameof(owner));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentOutOfRangeException.ThrowIfNegative(likeCount, nameof(likeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(dislikeCount, nameof(dislikeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(commentCount, nameof(commentCount));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new CommunityPost(id, communityName, owner, content, postType, publicType, restrictions, tags, createdAt, likeCount, dislikeCount, commentCount, communityId, appUserId);
    }
}
