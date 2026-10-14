using Communication.Domain.Aggregates;
using Communication.Domain.ValueObjects;

namespace Communication.Domain.Entities.Post;

public class UserPostComment
{
    public const int CONTENT_MAX_LENGTH = 256;

    private UserPostComment()
    {
    }

    private UserPostComment(string content, UserId appUserId, DateTimeOffset createdAt)
    {
        Content = content;
        CreatedAt = createdAt;
        AppUserId = appUserId;
    }

    public UserPostCommentId Id { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public UserPostId UserPostId { get; private set; }

    public UserId AppUserId { get; private set; }

    public UserPost UserPost { get; private set; }

    public static UserPostComment Create(string content, UserId appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));

        var createdAt = DateTimeOffset.UtcNow;
        return new UserPostComment(content, appUserId, createdAt);
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