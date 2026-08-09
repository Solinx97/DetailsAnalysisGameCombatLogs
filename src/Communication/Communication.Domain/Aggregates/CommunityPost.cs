using Communication.Domain.Entities.Post;
using Communication.Domain.Exceptions;

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

    private CommunityPost(string communityName, string owner, string content, int postType,
        int publicType, int restrictions, string tags,  DateTimeOffset createdAt,
        int likeCount, int dislikeCount, int commentCount, string appUserId)
    {
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

    public static CommunityPost Create(string communityName, string owner, string content, int postType, 
        int publicType, int restrictions, string tags, int likeCount,
        int dislikeCount, int commentCount, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(owner, nameof(owner));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentOutOfRangeException.ThrowIfNegative(likeCount, nameof(likeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(dislikeCount, nameof(dislikeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(commentCount, nameof(commentCount));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPost(communityName, owner, content, postType, publicType, restrictions, tags, createdAt, likeCount, dislikeCount, commentCount, appUserId);
    }

    public void AddLike(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var like = CommunityPostLike.Create(appUserId);
        _communityPostLikes.Add(like);
    }

    public void RemoveLike(int likeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(likeId, nameof(likeId));

        var like = _communityPostLikes
            .FirstOrDefault(x => x.Id == likeId)
                ?? throw new DomainException($"Community post like not found with id {likeId}");

        _communityPostLikes.Remove(like);
    }

    public void AddDislike(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var dislike = CommunityPostDislike.Create(appUserId);
        _communityPostDislikes.Add(dislike);
    }

    public void RemoveDislike(int dislikeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dislikeId, nameof(dislikeId));

        var dislike = _communityPostDislikes
            .FirstOrDefault(x => x.Id == dislikeId)
                ?? throw new DomainException($"Community post like not found with id {dislikeId}");

        _communityPostDislikes.Remove(dislike);
    }

    public void AddComment(string content, int commentType, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var comment = CommunityPostComment.Create(content, commentType, appUserId);
        _communityPostComments.Add(comment);
    }

    public void RemoveComment(int userPostCommentId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userPostCommentId, nameof(userPostCommentId));

        var comment = _communityPostComments
            .FirstOrDefault(x => x.Id == userPostCommentId)
                ?? throw new DomainException($"Community post comment not found with id {userPostCommentId}");

        _communityPostComments.Remove(comment);
    }

    public void EditContent(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        if (!string.Equals(Content, content, StringComparison.CurrentCultureIgnoreCase))
        {
            Content = content;
        }
    }

    public void EditCommentContent(int userPostId, string content)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userPostId, nameof(userPostId));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var comment = _communityPostComments
            .FirstOrDefault(x => x.Id == userPostId)
                ?? throw new DomainException($"Community post comment not not found with id {userPostId}");

        comment.EditContent(content);
    }
}
