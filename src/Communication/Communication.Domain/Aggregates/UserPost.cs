using Communication.Domain.Entities.Post;

namespace Communication.Domain.Aggregates;

public class UserPost
{
    private readonly List<UserPostComment> _userPostComments = [];
    private readonly List<UserPostDislike> _userPostDislikes = [];
    private readonly List<UserPostLike> _userPostLikes = [];

    private UserPost()
    {
    }

    private UserPost(int id, string owner, string content, int publicType,
        string tags, DateTimeOffset createdAt, int likeCount, int dislikeCount, 
        int commentCount, string appUserId)
    {
        Id = id;
        Owner = owner;
        Content = content;
        PublicType = publicType;
        Tags = tags;
        CreatedAt = createdAt;
        LikeCount = likeCount;
        DislikeCount = dislikeCount;
        CommentCount = commentCount;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Owner { get; private set; }

    public string Content { get; private set; }

    public int PublicType { get; private set; }

    public string Tags { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int LikeCount { get; private set; }

    public int DislikeCount { get; private set; }

    public int CommentCount { get; private set; }

    public string AppUserId { get; private set; }

    public IReadOnlyList<UserPostComment> UserPostComments => _userPostComments;

    public IReadOnlyList<UserPostDislike> UserPostDislikes => _userPostDislikes;

    public IReadOnlyList<UserPostLike> UserPostLikes => _userPostLikes;

    public static UserPost Create(int id, string owner, string content, int publicType,
        string tags, DateTimeOffset createdAt, int likeCount, int dislikeCount,
        int commentCount, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(owner, nameof(owner));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));
        ArgumentOutOfRangeException.ThrowIfNegative(likeCount, nameof(likeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(dislikeCount, nameof(dislikeCount));
        ArgumentOutOfRangeException.ThrowIfNegative(commentCount, nameof(commentCount));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new UserPost(id, owner, content, publicType, tags, createdAt, likeCount, dislikeCount, commentCount, appUserId);
    }
}
