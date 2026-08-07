using Communication.Domain.Aggregates;

namespace Communication.Domain.Entities.Post;

public class UserPostDislike
{
    private UserPostDislike()
    {
    }

    private UserPostDislike(int id, int userPostId, string appUserId)
    {
        Id = id;
        UserPostId = userPostId;
        AppUserId = appUserId;
    }

    public int Id { get; private set; }

    public int UserPostId { get; private set; }

    public string AppUserId { get; private set; }

    public UserPost UserPost { get; private set; }

    public static UserPostDislike Create(int id, int userPostId, string appUserId)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id, nameof(id));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userPostId, nameof(userPostId));
        ArgumentException.ThrowIfNullOrEmpty(appUserId, nameof(appUserId));

        return new UserPostDislike(id, userPostId, appUserId);
    }
}
