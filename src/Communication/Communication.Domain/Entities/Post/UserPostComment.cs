using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class UserPostComment
{
    private UserPostComment()
    {
    }

    private UserPostComment(string content, DateTimeOffset createdAt, string appUserId)
    {
        Content = content;
        CreatedAt = createdAt;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public int UserPostId { get; private set; }

    public string AppUserId { get; private set; }

    public UserPost UserPost { get; private set; }

    public static UserPostComment Create(string content, string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(content, nameof(content));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        var createdAt = DateTimeOffset.UtcNow;
        return new UserPostComment(content, createdAt, appUserId);
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