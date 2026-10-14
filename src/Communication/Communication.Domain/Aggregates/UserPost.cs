using Communication.Domain.Entities.Post;
using Communication.Domain.Enums;
using Communication.Domain.Exceptions;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Aggregates;

public class UserPost
{
    public const int CONTENT_MAX_LENGTH = 512;

    private readonly List<UserPostComment> _userPostComments = [];
    private readonly List<UserPostDislike> _userPostDislikes = [];
    private readonly List<UserPostLike> _userPostLikes = [];

    private UserPost()
    {
    }

    private UserPost(string content, int publicType, string tags,
        UserId appUserId, DateTimeOffset createdAt)
    {
        Content = content;
        PublicType = publicType;
        Tags = tags;
        AppUserId = appUserId;
        CreatedAt = createdAt;
    }

    public UserPostId Id { get; private set; }

    public string Content { get; private set; }

    public int PublicType { get; private set; }

    public string Tags { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public UserId AppUserId { get; private set; }

    public IReadOnlyList<UserPostComment> UserPostComments => _userPostComments;

    public IReadOnlyList<UserPostDislike> UserPostDislikes => _userPostDislikes;

    public IReadOnlyList<UserPostLike> UserPostLikes => _userPostLikes;

    public static UserPost Create(string content, int publicType, string tags, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var createdAt = DateTimeOffset.UtcNow;
        return new UserPost(content, publicType, tags, appUserId, createdAt);
    }

    public (UserPostLike, PostReactionStatus) AddLike(UserId appUserId)
    {
        var existLike = _userPostLikes
            .FirstOrDefault(x => x.AppUserId == appUserId);
        if (existLike != null)
        {
            var removedLike = RemoveLike(existLike.Id);

            return (removedLike, PostReactionStatus.RemoveLike);
        }
        else
        {
            var existDislike = _userPostDislikes
                .FirstOrDefault(x => x.AppUserId == appUserId);

            if (existDislike != null)
            {
                RemoveDislike(existDislike.Id);

                var like = UserPostLike.Create(appUserId);
                _userPostLikes.Add(like);

                return (like, PostReactionStatus.Like);
            }
            else
            {
                var like = UserPostLike.Create(appUserId);
                _userPostLikes.Add(like);

                return (like, PostReactionStatus.AddLike);
            }
        }
    }

    public (UserPostDislike, PostReactionStatus) AddDislike(UserId appUserId)
    {
        var existDislike = _userPostDislikes
            .FirstOrDefault(x => x.AppUserId == appUserId);
        if (existDislike != null)
        {
            var removedDislike = RemoveDislike(existDislike.Id);

            return (removedDislike, PostReactionStatus.RemoveDislike);
        }
        else
        {
            var existLike = _userPostLikes
                .FirstOrDefault(x => x.AppUserId == appUserId);

            if (existLike != null)
            {
                RemoveLike(existLike.Id);

                var dislike = UserPostDislike.Create(appUserId);
                _userPostDislikes.Add(dislike);

                return (dislike, PostReactionStatus.Dislike);
            }
            else
            {
                var dislike = UserPostDislike.Create(appUserId);
                _userPostDislikes.Add(dislike);

                return (dislike, PostReactionStatus.AddDislike);
            }
        }
    }

    public UserPostComment AddComment(string content, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var comment = UserPostComment.Create(content, appUserId);
        _userPostComments.Add(comment);

        return comment;
    }

    public void RemoveComment(int userPostCommentId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userPostCommentId, nameof(userPostCommentId));

        var comment = _userPostComments
            .FirstOrDefault(x => x.Id == userPostCommentId)
                ?? throw new DomainException($"User post comment not found with id {userPostCommentId}");

        _userPostComments.Remove(comment);
    }

    public void EditContent(string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        if (!string.Equals(Content, content, StringComparison.CurrentCultureIgnoreCase))
        {
            Content = content;
        }
    }

    public void EditCommentContent(UserPostId userPostId, string content)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var comment = _userPostComments
            .FirstOrDefault(x => x.Id == userPostId)
                ?? throw new DomainException($"User post comment not not found with id {userPostId}");

        comment.EditContent(content);
    }

    private UserPostLike RemoveLike(int likeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(likeId, nameof(likeId));

        var like = _userPostLikes
            .FirstOrDefault(x => x.Id == likeId)
                ?? throw new DomainException($"User post like not found with id {likeId}");

        _userPostLikes.Remove(like);

        return like;
    }

    private UserPostDislike RemoveDislike(int dislikeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dislikeId, nameof(dislikeId));

        var dislike = _userPostDislikes
            .FirstOrDefault(x => x.Id == dislikeId)
                ?? throw new DomainException($"User post like not found with id {dislikeId}");

        _userPostDislikes.Remove(dislike);

        return dislike;
    }
}
