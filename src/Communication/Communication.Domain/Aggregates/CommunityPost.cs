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
        int communityId, string appUserId)
    {
        CommunityName = communityName;
        Owner = owner;
        Content = content;
        PostType = postType;
        PublicType = publicType;
        Restrictions = restrictions;
        Tags = tags;
        CreatedAt = createdAt;
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

    public int CommunityId { get; private set; }

    public string AppUserId { get; private set; }

    public Community Community { get; private set; }

    public IReadOnlyList<CommunityPostComment> CommunityPostComments => _communityPostComments;

    public IReadOnlyList<CommunityPostDislike> CommunityPostDislikes => _communityPostDislikes;

    public IReadOnlyList<CommunityPostLike> CommunityPostLikes => _communityPostLikes;

    public static CommunityPost Create(string communityName, string owner, string content, int postType, 
        int publicType, int restrictions, string tags, int communityId,
        string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(owner, nameof(owner));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(communityId, nameof(communityId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPost(communityName, owner, content, postType, publicType, restrictions, tags, createdAt, communityId, appUserId);
    }

    public void AddLike(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var existLike = _communityPostLikes
            .FirstOrDefault(x => x.AppUserId == appUserId);
        if (existLike != null)
        {
            RemoveLike(existLike.Id);
        }
        else
        {
            var existDislike = _communityPostDislikes
                .FirstOrDefault(x => x.AppUserId == appUserId);

            if (existDislike != null)
            {
                RemoveDislike(existDislike.Id);
            }

            var like = CommunityPostLike.Create(appUserId);
            _communityPostLikes.Add(like);
        }
    }

    public void AddDislike(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var existDislike = _communityPostDislikes
            .FirstOrDefault(x => x.AppUserId == appUserId);
        if (existDislike != null)
        {
            RemoveDislike(existDislike.Id);
        }
        else
        {
            var existLike = _communityPostLikes
                .FirstOrDefault(x => x.AppUserId == appUserId);

            if (existLike != null)
            {
                RemoveLike(existLike.Id);
            }

            var dislike = CommunityPostDislike.Create(appUserId);
            _communityPostDislikes.Add(dislike);
        }
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

    private void RemoveLike(int likeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(likeId, nameof(likeId));

        var like = _communityPostLikes
            .FirstOrDefault(x => x.Id == likeId)
                ?? throw new DomainException($"Community post like not found with id {likeId}");

        _communityPostLikes.Remove(like);
    }

    private void RemoveDislike(int dislikeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dislikeId, nameof(dislikeId));

        var dislike = _communityPostDislikes
            .FirstOrDefault(x => x.Id == dislikeId)
                ?? throw new DomainException($"Community post like not found with id {dislikeId}");

        _communityPostDislikes.Remove(dislike);
    }
}
