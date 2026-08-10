using Communication.Domain.Entities.Post;
using Communication.Domain.Exceptions;

namespace Communication.Domain.Aggregates;

public class UserPost
{
    private readonly List<UserPostComment> _userPostComments = [];
    private readonly List<UserPostDislike> _userPostDislikes = [];
    private readonly List<UserPostLike> _userPostLikes = [];

    private UserPost()
    {
    }

    private UserPost(string owner, string content, int publicType, string tags, 
        DateTimeOffset createdAt, string appUserId)
    {
        Owner = owner;
        Content = content;
        PublicType = publicType;
        Tags = tags;
        CreatedAt = createdAt;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Owner { get; private set; }

    public string Content { get; private set; }

    public int PublicType { get; private set; }

    public string Tags { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public string AppUserId { get; private set; }

    public IReadOnlyList<UserPostComment> UserPostComments => _userPostComments;

    public IReadOnlyList<UserPostDislike> UserPostDislikes => _userPostDislikes;

    public IReadOnlyList<UserPostLike> UserPostLikes => _userPostLikes;

    public static UserPost Create(string owner, string content, int publicType, string tags, 
        string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(owner, nameof(owner));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new UserPost(owner, content, publicType, tags, createdAt, appUserId);
    }

    public void AddLike(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var like = UserPostLike.Create(appUserId);
        _userPostLikes.Add(like);
    }

    public void RemoveLike(int likeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(likeId, nameof(likeId));

        var like = _userPostLikes
            .FirstOrDefault(x => x.Id == likeId)
                ?? throw new DomainException($"User post like not found with id {likeId}");

        _userPostLikes.Remove(like);
    }

    public void AddDislike(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var dislike = UserPostDislike.Create(appUserId);
        _userPostDislikes.Add(dislike);
    }

    public void RemoveDislike(int dislikeId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dislikeId, nameof(dislikeId));

        var dislike = _userPostDislikes
            .FirstOrDefault(x => x.Id == dislikeId)
                ?? throw new DomainException($"User post like not found with id {dislikeId}");

        _userPostDislikes.Remove(dislike);
    }

    public void AddComment(string content, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var comment = UserPostComment.Create(content, appUserId);
        _userPostComments.Add(comment);
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

    public void EditCommentContent(int userPostId, string content)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userPostId, nameof(userPostId));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var comment = _userPostComments
            .FirstOrDefault(x => x.Id == userPostId)
                ?? throw new DomainException($"User post comment not not found with id {userPostId}");

        comment.EditContent(content);
    }
}
