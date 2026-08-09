using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class UserPostLike
{
    private UserPostLike()
    {
    }

    private UserPostLike(string appUserId)
    {
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public int UserPostId { get; private set; }

    public string AppUserId { get; private set; }

    public UserPost UserPost { get; private set; }

    public static UserPostLike Create(string appUserId)
    {
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new UserPostLike(appUserId);
    }
}
