using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class UserPostComment
{
    private UserPostComment()
    {
    }

    private UserPostComment(int id, string content, DateTimeOffset createdAt, int userPostId,
        string appUserId)
    {
        Id = id;
        Content = content;
        CreatedAt = createdAt;
        UserPostId = userPostId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int UserPostId { get; private set; }

    public string AppUserId { get; private set; }

    public UserPost UserPost { get; private set; }

    public static UserPostComment Create(int id, string content, DateTimeOffset createdAt, int userPostId,
        string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userPostId, nameof(userPostId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new UserPostComment(id, content, createdAt, userPostId, appUserId);
    }
}