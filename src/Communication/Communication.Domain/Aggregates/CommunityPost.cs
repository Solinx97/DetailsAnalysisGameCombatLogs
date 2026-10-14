using Communication.Domain.Entities.Post;
using Communication.Domain.Enums;
using Communication.Domain.Exceptions;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Aggregates;

public class CommunityPost
{
    public const int CONTENT_MAX_LENGTH = 512;

    private readonly List<CommunityPostComment> _communityPostComments = [];
    private readonly List<CommunityPostDislike> _communityPostDislikes = [];
    private readonly List<CommunityPostLike> _communityPostLikes = [];

    private CommunityPost()
    {
    }

    private CommunityPost(string content, int postType, int publicType, int restrictions, 
        string tags, int communityId, UserId appUserId, DateTimeOffset createdAt)
    {
        Content = content;
        PostType = postType;
        PublicType = publicType;
        Restrictions = restrictions;
        Tags = tags;
        CommunityId = communityId;
        AppUserId = appUserId;
        CreatedAt = createdAt;
    }

    public CommunityPostId Id { get; private set; }

    public string Content { get; private set; }

    public int PostType { get; private set; }

    public int PublicType { get; private set; }

    public int Restrictions { get; private set; }

    public string Tags { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public CommunityId CommunityId { get; private set; }

    public UserId AppUserId { get; private set; }

    public Community Community { get; private set; }

    public IReadOnlyList<CommunityPostComment> CommunityPostComments => _communityPostComments;

    public IReadOnlyList<CommunityPostDislike> CommunityPostDislikes => _communityPostDislikes;

    public IReadOnlyList<CommunityPostLike> CommunityPostLikes => _communityPostLikes;

    public static CommunityPost Create(string content, int postType, int publicType, int restrictions,
        string tags, CommunityId communityId, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var createdAt = DateTimeOffset.UtcNow;
        return new CommunityPost(content, postType, publicType, restrictions, tags, communityId, appUserId, createdAt);
    }

    public (CommunityPostLike, PostReactionStatus) AddLike(CommunityId communityId, UserId appUserId)
    {
        var existLike = _communityPostLikes
            .FirstOrDefault(x => x.AppUserId == appUserId);
        if (existLike != null)
        {
            var removedLike = RemoveLike(existLike.Id);

            return (removedLike, PostReactionStatus.RemoveLike);
        }
        else
        {
            var existDislike = _communityPostDislikes
                .FirstOrDefault(x => x.AppUserId == appUserId);

            if (existDislike != null)
            {
                RemoveDislike(existDislike.Id);

                var like = CommunityPostLike.Create(communityId, appUserId);
                _communityPostLikes.Add(like);

                return (like, PostReactionStatus.Like);
            }
            else
            {
                var like = CommunityPostLike.Create(communityId, appUserId);
                _communityPostLikes.Add(like);

                return (like, PostReactionStatus.AddLike);
            }
        }
    }

    public (CommunityPostDislike, PostReactionStatus) AddDislike(CommunityId communityId, UserId appUserId)
    {
        var existDislike = _communityPostDislikes
            .FirstOrDefault(x => x.AppUserId == appUserId);
        if (existDislike != null)
        {
            var removedDislike = RemoveDislike(existDislike.Id);

            return (removedDislike, PostReactionStatus.RemoveDislike);
        }
        else
        {
            var existLike = _communityPostLikes
                .FirstOrDefault(x => x.AppUserId == appUserId);

            if (existLike != null)
            {
                RemoveLike(existLike.Id);

                var dislike = CommunityPostDislike.Create(communityId, appUserId);
                _communityPostDislikes.Add(dislike);

                return (dislike, PostReactionStatus.Dislike);
            }
            else
            {
                var dislike = CommunityPostDislike.Create(communityId, appUserId);
                _communityPostDislikes.Add(dislike);

                return (dislike, PostReactionStatus.AddDislike);
            }
        }
    }

    public CommunityPostComment AddComment(string content, int commentType, CommunityId communityId, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var comment = CommunityPostComment.Create(content, commentType, communityId, appUserId);
        _communityPostComments.Add(comment);

        return comment;
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

    private CommunityPostLike RemoveLike(int likeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(likeId, nameof(likeId));

        var like = _communityPostLikes
            .FirstOrDefault(x => x.Id == likeId)
                ?? throw new DomainException($"Community post like not found with id {likeId}");

        _communityPostLikes.Remove(like);

        return like;
    }

    private CommunityPostDislike RemoveDislike(int dislikeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dislikeId, nameof(dislikeId));

        var dislike = _communityPostDislikes
            .FirstOrDefault(x => x.Id == dislikeId)
                ?? throw new DomainException($"Community post like not found with id {dislikeId}");

        _communityPostDislikes.Remove(dislike);

        return dislike;
    }
}
